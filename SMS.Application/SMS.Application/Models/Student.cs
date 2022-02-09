using System;
using System.Collections.Generic;

#nullable disable

namespace SMS.Application.Models
{
    public partial class Student
    {
        public Student()
        {
            Attendances = new HashSet<Attendance>();
            ClassStudents = new HashSet<ClassStudent>();
            ExerciseEvaluations = new HashSet<ExerciseEvaluation>();
        }

        public Guid StudentId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public int YearOfStudies { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<ClassStudent> ClassStudents { get; set; }
        public virtual ICollection<ExerciseEvaluation> ExerciseEvaluations { get; set; }
    }
}
