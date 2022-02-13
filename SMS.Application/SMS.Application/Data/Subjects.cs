using System;
using System.Collections.Generic;

namespace SMS.Application.Data
{
    public partial class Subjects
    {

        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Comment { get; set; }
        public int SubjectType { get; set; }
        public Guid ProfessorId { get; set; }
        public int YearOfStudies { get; set; }
        public int Semester { get; set; }
        public string Color { get; set; }
        public int NumberOfHours { get; set; }

        public virtual Professors Professor { get; set; }
    }
}
