using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School_Managment_System.Models
{
    public class StudentMarks
    {
        public int SubjectId { get; set; }
        public int Totalmarks { get; set; }
        public int MarksObtain { get; set; }
        public decimal? Percentage { get; set; }

        public string subjectName { get; set; }
    }
}