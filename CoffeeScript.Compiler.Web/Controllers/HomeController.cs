using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;

namespace CoffeeScript.Compiler.Web.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(VaryByParam = "script", Duration = 86400)]
        public JavaScriptResult Compile(string script)
        {
            string path = Server.MapPath(string.Format("~/Scripts/{0}.coffee", script));
            string text = System.IO.File.ReadAllText(path);
            string src = Utils.CoffeeScriptProcessor.Process(text);
            
            return JavaScript(src);
        }
    }
}
