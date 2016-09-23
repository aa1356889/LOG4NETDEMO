using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo.Demo;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Error1(string id)
        {
            int x = 0;
            var b = 2 / x;
            return null;
        }
        public ActionResult Error2(Student stu)
        {
            int x = 0;
            var b = 2 / x;
            return null;
        }
        public ActionResult Error3(Calsses classes)
        {
            int x = 0;
            var b = 2 / x;
            return null;
        }

    }
}
