using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NETAuthentication.UserInterface.Mvc.Controllers
{
    public class PersonController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Persons Management";
            return View();
        }

        [HttpGet]
        public ActionResult Search()
        {
            ViewBag.Title = "Persons Search";
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "Person Create";
            return View();
        }
    }
}