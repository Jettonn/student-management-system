using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Lesson
{
    public class OnlineLessonViewModel
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string ClassComment { get; set; }
        public string LessonDate { get; set; }
        public List<StudentAttendanceViewModel> attendanceViewModels { get; set; }
    }
}
