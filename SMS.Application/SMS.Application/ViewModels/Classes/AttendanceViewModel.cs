using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Classes
{
    public class AttendanceViewModel
    {
        public List<AttendanceDto> attendanceDtos { get; set; }
    }

    public class AttendanceDto
    {
        public Guid LessonId { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
        public int Hours { get; set; }
        public int Absent { get; set; }
        public int Present { get; set; }
    }
}
