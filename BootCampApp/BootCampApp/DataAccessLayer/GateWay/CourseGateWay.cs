using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootCampApp.DataAccessLayer.DataAccessObject;

namespace BootCampApp.DataAccessLayer.GateWay
{
    class CourseGateWay
    {
        private SqlConnection connection;

        public CourseGateWay()
        {
            string conn = ConfigurationManager.ConnectionStrings["BootCamp"].ConnectionString;
            connection = new SqlConnection();
            connection.ConnectionString = conn;
        }
        public List<Course> GetCourseList()
        {

            connection.Open();
            string query = string.Format("SELECT * FROM t_Course");
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader aReader = command.ExecuteReader();
            List<Course> courses = new List<Course>();
            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    Course course = new Course();
                    course.CourseId = (int)aReader[0];
                    course.CourseTitle = aReader[1].ToString();
                    courses.Add(course);
                }
            }
            connection.Close();
            return courses;
        }
    }
}
