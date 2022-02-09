using System;
using System.Collections.Generic;

#nullable disable

namespace SMS.Application.Models
{
    public partial class Class
    {
        public Class()
        {
            ClassEvaluations = new HashSet<ClassEvaluation>();
            ClassStudents = new HashSet<ClassStudent>();
            Lessons = new HashSet<Lesson>();
        }

        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public string Comment { get; set; }
        public Guid SubjectId { get; set; }
        public Guid ProfessorId { get; set; }
        public int PlannedHours { get; set; }

        public virtual Professor Professor { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<ClassEvaluation> ClassEvaluations { get; set; }
        public virtual ICollection<ClassStudent> ClassStudents { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
