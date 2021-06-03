using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School_Managment_System.Models
{
    public class Result
    {
        public string Name { get; set; }
        public int RollNo { get; set; }
        public int Class { get; set; }

        public string  ExamType { get; set; }

        public decimal percentage { get; set; }

        public int Total { get; set; }
        public int Obtain { get; set; }

        public List<StudentMarks> ListofStudetnMarks { get; set; }
    }
}