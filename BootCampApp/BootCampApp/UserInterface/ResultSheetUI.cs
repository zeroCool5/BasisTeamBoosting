using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BootCampApp.BusinessLogicLayer;
using BootCampApp.DataAccessLayer.DataAccessObject;
using BootCampApp.DataAccessLayer.View;

namespace BootCampApp.UserInterface
{
    public partial class ResultSheetUI : Form
    {
        public ResultSheetUI()
        {
            InitializeComponent();
        }
        DateTime aDateTime = new DateTime();
        private Student aStudent;
        private StudentBll aStudentBll;
        
        private List<Enrollment> studentResultLIsts;
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
                ShowResultData(aStudent.RegNo);
                averageResultTextBox.Text = aStudentBll.ShowAverage(aStudent.RegNo).ToString();
                GradeLetterTextBox.Text = aStudentBll.ShowGrade();
            }
            catch (Exception)
            {
                MessageBox.Show(@"This Registration number is not valid");
            }
        }

        private void ShowResultData(string regNo)
        {
            studentResultLIsts = aStudentBll.GetStudentResultEnrollment(regNo);
            enrolledResultDataGridView.DataSource = studentResultLIsts;
        }
    }
}
