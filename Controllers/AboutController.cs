using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace School_Managment_System.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Index(int rollno)
        {
            return Content("Hello " + rollno);
        }
    }
}