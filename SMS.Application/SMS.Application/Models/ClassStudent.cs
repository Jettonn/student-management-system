using System;
using System.Collections.Generic;

#nullable disable

namespace SMS.Application.Models
{
    public partial class ClassStudent
    {
        public Guid ClassStudentsId { get; set; }
        public Guid ClassId { get; set; }
        public Guid StudentId { get; set; }
        public int StudentsHours { get; set; }

        public virtual Class Class { get; set; }
        public virtual Student Student { get; set; }
    }
}
