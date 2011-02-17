using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CoffeeScript.Compiler;

namespace ncoffee
{
    class ChangeWatcher
    {
        private readonly object sync = new object();
        private readonly IDictionary<string, DateTime> lastWrites = new Dictionary<string, DateTime>();
        private readonly FileSystemWatcher watcher;
        private readonly Action<string,CompilerOptions> action;
        private readonly CompilerOptions options;

        public ChangeWatcher(string path, CompilerOptions options, Action<string,CompilerOptions> action)
        {
            //Filesystemwatcher watches..directories. So if the path is a file we watch the file dir
            var isDirectory = (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
            var subDirs = isDirectory;
            
            string toWatch = path;
            string filter = "*.coffee";

            if (!isDirectory)
            {
                var info = new FileInfo(path);
                toWatch = info.DirectoryName;
                filter = info.Name;
            }

            watcher = new FileSystemWatcher(toWatch, filter) { IncludeSubdirectories = subDirs };
            this.action = action;
            this.options = options;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += OnChanged;
        }

        DateTime GetLastWrite(string path)
        {
            DateTime last;
                if (! lastWrites.TryGetValue(path,out last))
                    last = new DateTime(1,1,1);
            return last;
        }

        void SetLastWrite(string path, DateTime last)
        {
                lastWrites[path] = last;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
        }

        //FIXME:This isn't really safe, a script could change, start compiling
        //then change again and complete the second compilation first.
        //The latest compilation would be overwritten...
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var path = e.FullPath;
            var current = new FileInfo(path).LastWriteTime;
            
            lock (sync) //We need to serialize access here
            {
                var last = GetLastWrite(path);
                //hacky way to ignore multiple changes
                //inherent to using a fsw
                if (current.Subtract(last).TotalMilliseconds < 100)   
                    return;
                SetLastWrite(path, current);
            }
            action(e.FullPath, options);
        }
    }
}