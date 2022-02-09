using System;
using System.Collections.Generic;

#nullable disable

namespace SMS.Application.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            Attendances = new HashSet<Attendance>();
        }

        public Guid LessonId { get; set; }
        public Guid ClassId { get; set; }
        public DateTime Date { get; set; }
        public int Hours { get; set; }
        public string Comment { get; set; }

        public virtual Class Class { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
