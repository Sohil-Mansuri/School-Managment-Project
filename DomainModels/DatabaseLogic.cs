using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using School_Managment_System.ViewModels;
using School_Managment_System.Models;
using System.Web.Mvc;

namespace School_Managment_System.DomainModels
{
    public class DatabaseLogic
    {

        //Common Part
        string Constring = @"Data Source=FLAIRDEV-PC; Initial Catalog=SchoolManagment; Integrated Security=True";
        string Query;


        public IEnumerable<StudenExam> GetExamTypeList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection sqlcon = new SqlConnection(Constring))
            {
                sqlcon.Open();
                Query = "SELECT * FROM Exam";
                SqlDataAdapter sqldt = new SqlDataAdapter(Query, sqlcon);
                sqldt.Fill(dt);
            }

            var ExamList = new List<StudenExam>();
            foreach (DataRow dr in dt.Rows)
            {
                ExamList.Add(new StudenExam
                {
                    Id = Convert.ToInt32(dr["ExampId"]),
                    ExamType = dr["ExampName"].ToString()
                });



            }
            
            return ExamList;
        }
        public IEnumerable<Subject> GetStudentSubjectList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection sqlcon=new SqlConnection(Constring))
            {
                sqlcon.Open();
                Query = "SELECT * FROM Subject";
                SqlDataAdapter sqldt = new SqlDataAdapter(Query, sqlcon);
                sqldt.Fill(dt);

                

                
            }
            var subjectList = new List<Subject>();

            foreach (DataRow dr in dt.Rows)
            {
                subjectList.Add(new Subject
                {

                    Id=Convert.ToInt32(dr["SubjectId"]),
                    StudentSubject=dr["SubjectName"].ToString()

                });

            }
            return subjectList;

        }

        public int  AddStudetnDetailsIntoDatabse(StudentDetails stuDetails)
        {
            DataTable dt = new DataTable();

            using (SqlConnection sqlcon=new SqlConnection(Constring))
            {
                sqlcon.Open();

                Query = "INSERT INTO StudentDetails VALUES(@name,@examId,@class,@Rollno)";

                SqlCommand sqlc = new SqlCommand(Query, sqlcon);
                sqlc.Parameters.AddWithValue("@name", stuDetails.StudentName);
                sqlc.Parameters.AddWithValue("@examId", stuDetails.ExamId);
                sqlc.Parameters.AddWithValue("@class", stuDetails.ClassName);
                sqlc.Parameters.AddWithValue("@Rollno", stuDetails.RollNo);
                sqlc.ExecuteNonQuery();

                string query2 = "SELECT StudentId FROM StudentDetails WHERE RollNumber=@rollno";
                SqlDataAdapter sqldt = new SqlDataAdapter(query2, sqlcon);
                sqldt.SelectCommand.Parameters.AddWithValue("@rollno", stuDetails.RollNo);

                sqldt.Fill(dt);

            }
            return Convert.ToInt32(dt.Rows[0][0]);

        }
        public void AddStudentMarksIntoDatabase(List<StudentMarks>  StuMarks, int Stuid)
        {
            decimal total = 0;
            decimal obtain = 0;
            decimal percentage;


            using (SqlConnection sqlcon=new SqlConnection(Constring))
            {
                sqlcon.Open();



                foreach (var item in StuMarks)
                {

                    Query = "INSERT INTO StudentMarks VALUES(@stuid,@subid,@totalmarks,@marksobtain,@percentage)";
                    SqlCommand sqlc = new SqlCommand(Query, sqlcon);
                    sqlc.Parameters.AddWithValue("@stuid", Stuid);

                    sqlc.Parameters.AddWithValue("@subid", item.SubjectId);
                    sqlc.Parameters.AddWithValue("@totalmarks", item.Totalmarks);
                    sqlc.Parameters.AddWithValue("@marksobtain", item.MarksObtain);
                    sqlc.Parameters.AddWithValue("@percentage", item.Percentage);

                    sqlc.ExecuteNonQuery();
                }

                //counting percentge and storing into database

                foreach(var item in StuMarks)
                {
                    total = total + item.Totalmarks;
                    obtain = obtain + item.MarksObtain;

                }
                percentage = (obtain / total) * 100;

                string query2 = "INSERT INTO StudentPercentage VALUES(@id,@percentage,@total,@obtain)";

                SqlCommand sql = new SqlCommand(query2, sqlcon);
                sql.Parameters.AddWithValue("@id", Stuid);
                sql.Parameters.AddWithValue("@percentage", percentage);
                sql.Parameters.AddWithValue("@total", total);
                sql.Parameters.AddWithValue("@obtain", obtain);

                sql.ExecuteNonQuery();




            }




        }
        public  string Getsubject(int id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection sqlcon=new SqlConnection(Constring))
            {
                sqlcon.Open();
              string Query = "SELECT SubjectName FROM Subject WHERE SubjectId=@id";
                SqlDataAdapter sdt = new SqlDataAdapter(Query, sqlcon);
                sdt.SelectCommand.Parameters.AddWithValue("@id", id);
                sdt.Fill(dt);

            }
            return dt.Rows[0][0].ToString();

        }

        public string GetExampType(int id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection sqlcon=new SqlConnection(Constring))
            {
                sqlcon.Open();
                Query = "SELECT ExampName FROM Exam WHERE ExampId=@id";
                SqlDataAdapter sql = new SqlDataAdapter(Query, sqlcon);
                sql.SelectCommand.Parameters.AddWithValue("@id", id);

                sql.Fill(dt);
            }
            return dt.Rows[0][0].ToString();
        }
        public Result GetResult(int rollno)
        {
            var Resultobject = new Result();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            using (SqlConnection sqlcon = new SqlConnection(Constring))
            {
                sqlcon.Open();
                string query1 = "SELECT * FROM StudentDetails JOIN StudentPercentage ON StudentDetails.StudentId = StudentPercentage.StudentId where RollNumber=@rollno";
                SqlDataAdapter sql = new SqlDataAdapter(query1, sqlcon);
                sql.SelectCommand.Parameters.AddWithValue("@rollno", rollno);
                sql.Fill(dt1);



                if (dt1.Rows.Count == 1)
                {
                    string query2 = "SELECT * FROM StudentMarks WHERE StudetnId=@id";
                    SqlDataAdapter sql2 = new SqlDataAdapter(query2, sqlcon);
                    sql2.SelectCommand.Parameters.AddWithValue("@id", dt1.Rows[0][0]);
                    sql2.Fill(dt2);

                }

                Resultobject.Name = dt1.Rows[0][1].ToString();
                Resultobject.RollNo = Convert.ToInt32(dt1.Rows[0][4]);
                Resultobject.Class = Convert.ToInt32(dt1.Rows[0][3]);

                var listofstudentMarks = new List<StudentMarks>();

                foreach (DataRow dr in dt2.Rows)                               //getting marks 
                {
                    listofstudentMarks.Add(new StudentMarks
                    {
                        SubjectId = Convert.ToInt32(dr["SubjectId"]),
                        Totalmarks = Convert.ToInt32(dr["TotalMarks"]),
                        MarksObtain = Convert.ToInt32(dr["MarksObtain"]),
                        subjectName=Getsubject(Convert.ToInt32(dr["SubjectId"]))

                     });

                }
                Resultobject.ListofStudetnMarks = listofstudentMarks;

                Resultobject.ExamType = GetExampType(Convert.ToInt32(dt1.Rows[0][2]));
                Resultobject.percentage = Math.Round(Convert.ToDecimal(dt1.Rows[0][7]), 2);
                Resultobject.Total = Convert.ToInt32(dt1.Rows[0][8]);
                Resultobject.Obtain = Convert.ToInt32(dt1.Rows[0][9]);

            }
            return Resultobject;














        }
        public bool CheckRollNo(int rollno)
        {
            DataTable dt = new DataTable();
            using (SqlConnection sqlcon=new SqlConnection(Constring))
            {
                sqlcon.Open();
                Query = "SELECT RollNumber FROM StudentDetails WHERE RollNumber=@rollno";              
                SqlDataAdapter sql = new SqlDataAdapter(Query, sqlcon);
                sql.SelectCommand.Parameters.AddWithValue("@rollno", rollno);
                sql.Fill(dt);

            }
            if (dt.Rows.Count == 1)
            {
                return true;
            }
            else
                return false;

        }


    }
 }   
