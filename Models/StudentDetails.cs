using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School_Managment_System.Models
{
    public class StudentDetails
    {
        public string StudentName { get; set; }
        public int ClassName { get; set; }
        public int ExamId { get; set; }
        public string RollNo { get; set; }

        public List<StudentMarks> ListOfStudtnMarks { get; set; }
    }
}