using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootCampApp.DataAccessLayer.DataAccessObject;
using BootCampApp.DataAccessLayer.GateWay;
using BootCampApp.DataAccessLayer.View;

namespace BootCampApp.BusinessLogicLayer
{
    class StudentBll
    {
        public  StudentGateWay aStudentGateWay;
        public CourseGateWay aCourseGateWay;
        public List<Student> Students { set; get; }
        public List<Course> Courses { set; get; }

        private Student aStudent;
        private string msg="";

        public StudentBll()
        {
            aStudentGateWay = new StudentGateWay();
            aCourseGateWay = new CourseGateWay();
        }

        public Student SearchRegNo(string regNo)
        {
            if (HasThisStudent(regNo))
            {
                aStudent = aStudentGateWay.HasStudentInformation(regNo);
                
            }
            return aStudent;
        }

        public bool HasThisStudent(string regNo)
        {
           return aStudentGateWay.HasThisStudent(regNo);
        }

        public string EnrollThisRegNo(Student student, DateTime aDateTime)
        {
            if (HasThisRegNo(student))
            {
                return "This " + aStudent.RegNo + " has already been enrolled in " + aStudent.CourseId;
            }
            else
            {
                return aStudentGateWay.SaveEnroll(aStudent, aDateTime);
            }
        }

        private bool HasThisRegNo(Student aStudent)
        {
            return aStudentGateWay.HasThisRegNo(aStudent);
        }

        public List<Enrollment> GetStudentCourseEnrollment(string student)
        {
            return aStudentGateWay.GetStudentCourseEnrollment(student);
        }

        public string EntryPercentageOfThisCourse(Student aStudent, double score, DateTime aDateTime)
        {
            if (HasThisCourseEntry(aStudent))
            {
                return "Score for Reg. No.: " + aStudent.RegNo + " is already in sytem\n for Course ID: " + aStudent.CourseId;
            }
            else
            {
                return aStudentGateWay.EntryTheScore(aStudent, score, aDateTime);
            }
        }

        private bool HasThisCourseEntry(Student aStudent)
        {
            return aStudentGateWay.HasThisCourseEntry(aStudent);
        }
    }
}
