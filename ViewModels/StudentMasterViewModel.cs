using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace School_Managment_System.ViewModels
{
    public class StudentMasterViewModel
    {
        [Display(Name ="Exam Type")]
        public int ExamId { get; set; }

        public IEnumerable<SelectListItem> ExamType { get; set; }

        [Display(Name ="Subject")]
        public int SubjectId { get; set; }

        public IEnumerable<SelectListItem> SubjectType { get; set; }

        [Display(Name ="Full Name")]
        [Required(ErrorMessage ="Plese Enter Your Name")]
        public string Name { get; set; }
        [Display(Name ="Roll No")]
        public int RollNo { get; set; }


        [Display(Name ="STD")]
        public int className { get; set; }

        [Display(Name ="Marks Obtain")]
        public int MarksObtain { get; set; }

        [Display(Name = "Total Marks")]
        public int TotalMarks { get; set; }

        public decimal Percentage { get; set; }


    }
}