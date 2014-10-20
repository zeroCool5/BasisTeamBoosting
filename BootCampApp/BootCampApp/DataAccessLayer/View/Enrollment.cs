using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCampApp.DataAccessLayer.View
{
    class Enrollment
    {
        public string StudentRegNo { set; get; }
        public int CourseId { set; get; }
        public float Result { get; set; }
        public DateTime ADateTime { get; set; }
    }
}
