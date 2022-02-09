using System;
using System.Collections.Generic;

#nullable disable

namespace SMS.Application.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Classes = new HashSet<Class>();
        }

        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Comment { get; set; }
        public int SubjectType { get; set; }
        public Guid ProfessorId { get; set; }
        public int YearOfStudies { get; set; }
        public int Semester { get; set; }
        public string Color { get; set; }
        public int NumberOfHours { get; set; }

        public virtual Professor Professor { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
    }
}
