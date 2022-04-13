using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CNet.Web.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("hello webapi");
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
