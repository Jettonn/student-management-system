using System;
using System.Collections.Generic;

#nullable disable

namespace SMS.Application.Models
{
    public partial class Attendance
    {
        public Guid AttendanceId { get; set; }
        public Guid LessonId { get; set; }
        public Guid StudentId { get; set; }
        public int AttendaceStatus { get; set; }

        public virtual Lesson Lesson { get; set; }
        public virtual Student Student { get; set; }
    }
}
