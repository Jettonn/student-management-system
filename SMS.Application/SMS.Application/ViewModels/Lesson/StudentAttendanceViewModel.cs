using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Lesson
{
    public class StudentAttendanceViewModel
    {
        public Guid AttendanceId { get; set; }
        public string StudentName { get; set; }
        public int AttendanceStatus { get; set; }
    }

}