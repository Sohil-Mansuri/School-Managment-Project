using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using School_Managment_System.DomainModels;
using School_Managment_System.ViewModels;
using System.Data;
using School_Managment_System.Models;

namespace School_Managment_System.Controllers
{
     
    public class HomeController : Controller
    {
        DatabaseLogic DbAccess;
        DataTable dt;

        public HomeController()
        {
            DbAccess = new DatabaseLogic();
        }

        public ActionResult Index()
        {

              
            var StudentObj = new StudentMasterViewModel();
            StudentObj.ExamType = DbAccess.GetExamTypeList().Select(x => new SelectListItem
            {
                Text = x.ExamType,
                Value = x.Id.ToString()
            });
            StudentObj.SubjectType = DbAccess.GetStudentSubjectList().Select(x => new SelectListItem
            {
                Text = x.StudentSubject,
                Value = x.Id.ToString()
            });




               
            return View(StudentObj);
        }
        [HttpPost]
        public JsonResult AddStudentDetailsIntoDatabase(StudentDetails studetain)
        {

             var studetnId=DbAccess.AddStudetnDetailsIntoDatabse(studetain);


            DbAccess.AddStudentMarksIntoDatabase(studetain.ListOfStudtnMarks,studetnId);
            return Json(new {message="Data Successfully Added",status="OK" },JsonRequestBehavior.AllowGet);
        }

        public ActionResult Result()
        {
            return View();
        }
        [HttpPost]
        public PartialViewResult Result(int rollno)
        {
            var Result = new Result();
           Result= DbAccess.GetResult(rollno);

            return PartialView("_StudentMarksTable", Result);
        }

        public JsonResult CheckRollNo(int RollNo)
        {

            var valid = DbAccess.CheckRollNo(RollNo);
            return Json(valid,JsonRequestBehavior.AllowGet);
        }
    }
}