using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BootCampApp.BusinessLogicLayer;
using BootCampApp.DataAccessLayer.DataAccessObject;
using BootCampApp.DataAccessLayer.GateWay;
using BootCampApp.DataAccessLayer.View;

namespace BootCampApp.UserInterface
{
    public partial class CourseEnrollmentUI : Form
    {
        public CourseEnrollmentUI()
        {
            InitializeComponent();
        }

        DateTime aDateTime = new DateTime();
        private Student aStudent;
        private StudentBll aStudentBll;
        CourseBll aCourseBll = new CourseBll();
        Course aCourse=new Course();
        private List<Enrollment> studentCourseLIsts;

        private void findButton_Click(object sender, EventArgs e)
        {
            try
            {
                aStudent = new Student();
                aStudentBll = new StudentBll();
                aStudent = aStudentBll.SearchRegNo(regnoTextBox.Text);

                aStudent.RegNo = regnoTextBox.Text;
                nameTextBox.Text = aStudent.Name;
                emailTextBox.Text = aStudent.Email;
                ShowCourseComboBox();
            }
            catch (Exception)
            {
                MessageBox.Show(@"This Registration number is not valid");
            }
        }

        private void ShowCourseComboBox()
        {
            List<Course> courses = aCourseBll.GetCourseList();
            foreach (Course aCourse in courses)
            {
                courseComboBox.Items.Add(aCourse);
            }
            courseComboBox.DisplayMember = "CourseTitle";
            courseComboBox.ValueMember = "CourseId";
        }

        private void enrollButton_Click(object sender, EventArgs e)
        {
            aCourse = (Course)courseComboBox.SelectedItem;
            aStudent.CourseId = aCourse.CourseId;
            aDateTime = enrollmentDateTimePicker.Value.Date;
            string msg = aStudentBll.EnrollThisRegNo(aStudent, aDateTime);
            
            ShowData(aStudent.RegNo);
            MessageBox.Show(msg);
        }

        private void ShowData(string regNo)
        {
            studentCourseLIsts = aStudentBll.GetStudentCourseEnrollment(regNo);
            enrolledCoursesDataGridView.DataSource = studentCourseLIsts;
        }
    }
}
