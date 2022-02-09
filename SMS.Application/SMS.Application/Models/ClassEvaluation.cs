using System;
using System.Collections.Generic;

#nullable disable

namespace SMS.Application.Models
{
    public partial class ClassEvaluation
    {
        public ClassEvaluation()
        {
            ExerciseEvaluations = new HashSet<ExerciseEvaluation>();
        }

        public Guid ClassEvaluationId { get; set; }
        public Guid ClassId { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public int Type { get; set; }

        public virtual Class Class { get; set; }
        public virtual ICollection<ExerciseEvaluation> ExerciseEvaluations { get; set; }
    }
}
