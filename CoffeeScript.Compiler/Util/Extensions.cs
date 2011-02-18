using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CoffeeScript.Compiler.Util
{
    public static class FileSystemAndStringExtensions
    {


        public static DirectoryInfo AsDirectory(this string self)
        {
            if (self.IsNullOrWhiteSpace())
                throw new ArgumentException("directory name can't be null or empty","self");
            return new DirectoryInfo(self);
        }

        ///<summary>returns true if me is null, empty or only whitespace</summary>
        public static bool IsNullOrWhiteSpace(this string me)
        {
            return (me == null || me.Trim().Length == 0);
        }

        /// <summary>
        /// Returns a DirectoryInfo that pointing at a directory that is found by 
        /// following <paramref name="relativePath"/> from <paramref name="me"/>
        /// This folder may or may not exist.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static DirectoryInfo SubDir(this DirectoryInfo me, string relativePath)
        {
            if (me == null || string.IsNullOrEmpty(me.FullName))
                throw new ArgumentException("Directory path can't be null or empty","me");
            if (me.FullName.Length==0)
                throw new ArgumentException("The directory must have a full path","me");
            if (relativePath.IsNullOrWhiteSpace())
                throw new ArgumentException("Relative path can't be null or empty", "relativePath");
            
            if (relativePath.Substring(0,1) == "\\")
                relativePath = relativePath.Remove(0, 1);
            
            return Path.Combine(me.FullName, relativePath).AsDirectory();
        }

        /// <summary>
        /// Returns a DirectoryInfo that pointing at a directory that is found by 
        /// following <paramref name="filePath"/> from <paramref name="me"/>.
        /// This file may or may not exist.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static FileInfo File(this DirectoryInfo me, string filePath)
        {
            if (me == null)
                throw new ArgumentException("Directory path can't be null", "me");
            if (filePath.IsNullOrWhiteSpace())
                throw new ArgumentException("Relative path can't be null or empty", "filePath");
            if (filePath.Substring(0, 1) == "\\")
            {
                filePath = filePath.Remove(0, 1);
            }
            return new FileInfo(Path.Combine(me.FullName, filePath));
        }


        public static IEnumerable<FileInfo> Glob(this DirectoryInfo directory, string pattern)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            foreach (var file in directory.GetFiles(pattern))
                yield return file;
            foreach (var child in directory.GetDirectories().SelectMany(dir => Glob(dir, pattern)))
                yield return child;
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
                action(item);
        }   






    }
}
