using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BootCampApp.DataAccessLayer.DataAccessObject;
using BootCampApp.DataAccessLayer.View;

namespace BootCampApp.DataAccessLayer.GateWay
{
    class StudentGateWay
    {
        private SqlConnection connection;
        public StudentGateWay()
        {
            string conn = ConfigurationManager.ConnectionStrings["BootCamp"].ConnectionString;
            connection = new SqlConnection();
            connection.ConnectionString = conn;
        }

        public string SaveEnroll(Student aStudent, DateTime sysDateTime)
        {
            connection.Open();
            string query = string.Format("INSERT INTO t_StudentEnroll VALUES(@aNewReg, @aNewCourse,@aNewDate)");
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add(new SqlParameter("@aNewReg", aStudent.RegNo));
            command.Parameters.Add(new SqlParameter("@aNewCourse", aStudent.CourseId));
            command.Parameters.Add(new SqlParameter("@aNewDate", sysDateTime));

            int affectedRows = command.ExecuteNonQuery();
            connection.Close();
            if (affectedRows > 0)
                return "Student enrollment has been saved to database";
            return "Something went wrong";
        }

        public bool HasThisStudent(string regNo)
        {
            connection.Open();
            string query = string.Format("SELECT * FROM t_Student WHERE Student_RegNo=@aNewID");
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add(new SqlParameter("@aNewID", regNo));
            SqlDataReader aReader = command.ExecuteReader();
            bool Hasrow = aReader.HasRows;
            connection.Close();
            return Hasrow;
        }

        public Student HasStudentInformation(string regNo)
        {
            Student aStudent=new Student();
            connection.Open();
            string query = string.Format("SELECT * FROM t_Student WHERE Student_RegNo=@aNewID");
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add(new SqlParameter("@aNewID", regNo));
           using (SqlDataReader aReader = command.ExecuteReader())
                {
                    while (aReader.Read())
                    {
                        aStudent.Name = aReader["Student_Name"].ToString();
                        aStudent.Email = aReader["Student_Email"].ToString();                       
                    }

                    connection.Close();
                }               
            
            return aStudent;
        }

        public bool HasThisRegNo(Student aStudent)
        {
            connection.Open();
            string query = string.Format("SELECT * FROM t_StudentEnroll WHERE Student_RegNo=@aNewId AND Course_Id=@aNewCourse");
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add(new SqlParameter("@aNewId", aStudent.RegNo));
            command.Parameters.Add(new SqlParameter("@aNewCourse", aStudent.CourseId));
            SqlDataReader aReader = command.ExecuteReader();
            bool Hasrow = aReader.HasRows;
            connection.Close();
            return Hasrow;
        }

        public List<Enrollment> GetStudentCourseEnrollment(string regNo)
        {
            connection.Open();
            string query = "SELECT * FROM t_StudentEnroll WHERE Student_RegNo='" + regNo + "'";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader aReader = command.ExecuteReader();
            List<Enrollment> aViews = new List<Enrollment>();
            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    Enrollment anEnrollment = new Enrollment();

                    anEnrollment.StudentRegNo = aReader[0].ToString();
                    anEnrollment.CourseId = (int)aReader[1];
                    anEnrollment.ADateTime = (DateTime) aReader[2];
                    aViews.Add(anEnrollment);
                }
            }
            connection.Close();
            return aViews;
        }

        public bool HasThisCourseEntry(Student aStudent)
        {
            connection.Open();
            string query = string.Format("SELECT * FROM t_Score WHERE Student_RegNo=@aNewId AND Course_Id=@aNewCourse");
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add(new SqlParameter("@aNewId", aStudent.RegNo));
            command.Parameters.Add(new SqlParameter("@aNewCourse", aStudent.CourseId));
            SqlDataReader aReader = command.ExecuteReader();
            bool Hasrow = aReader.HasRows;
            connection.Close();
            return Hasrow;
        }

        public string EntryTheScore(Student aStudent, double score, DateTime sysDateTime)
        {
            connection.Open();
            string checkEnrollment = string.Format("SELECT Student_RegNo from t_StudentEnroll WHERE Student_RegNo=@aNewReg AND Course_Id=@aNewCourse");
            SqlCommand command = new SqlCommand(checkEnrollment, connection);
            command.Parameters.Add(new SqlParameter("@aNewReg", aStudent.RegNo));
            command.Parameters.Add(new SqlParameter("@aNewCourse", aStudent.CourseId));
            SqlDataReader aReader = command.ExecuteReader();
            
            if (aReader.HasRows)
            {   connection.Close();
                connection.Open();
                    string query = string.Format("INSERT INTO t_Score VALUES(@aNewReg, @aNewCourse,@anScore, @aNewDate)");
                    SqlCommand aCommand = new SqlCommand(query, connection);
                    aCommand.Parameters.Add(new SqlParameter("@aNewReg", aStudent.RegNo));
                    aCommand.Parameters.Add(new SqlParameter("@aNewCourse", aStudent.CourseId));
                    aCommand.Parameters.Add(new SqlParameter("@anScore", score));
                    aCommand.Parameters.Add(new SqlParameter("@aNewDate", sysDateTime));
                    aCommand.ExecuteNonQuery();
                    connection.Close();
                return "Successfully stored the score for " + aStudent.RegNo;
            }

            else
            {
                connection.Close();
                return "Your Data Can't be saved since the id: " + aStudent.RegNo + " is not Enrolled for the course " +
                       aStudent.CourseId;
            }

        }

        public List<Enrollment> GetStudentResultEnrollment(string regNo)
        {
            connection.Open();
            string query = "SELECT * FROM t_Score WHERE Student_RegNo='" + regNo + "'";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader aReader = command.ExecuteReader();
            List<Enrollment> aViews = new List<Enrollment>();
            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    Enrollment anEnrollment = new Enrollment();
                    anEnrollment.StudentRegNo = aReader[1].ToString();
                    anEnrollment.CourseId = (int)aReader[2];
                    anEnrollment.Result = (double) aReader[3];
                    anEnrollment.ADateTime = (DateTime)aReader[4];
                    aViews.Add(anEnrollment);
                }
            }
            connection.Close();
            return aViews;
        }

        public double ShowAverage(string regNo)
        {
            double getAverage = 0;
            connection.Open();
            
            string query = "SELECT AVG(Course_Score) AS Average FROM t_Score WHERE Student_RegNo='" + regNo + "'";
            SqlCommand command = new SqlCommand(query, connection);
            using (SqlDataReader aReader = command.ExecuteReader())
            {
                
                while (aReader.Read())
                {
                    getAverage = (double)aReader["Average"];
                }

                connection.Close();
                
            }
            return getAverage;
            

        }
    }
}
