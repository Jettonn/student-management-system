using System;
using System.Collections.Generic;

#nullable disable

namespace SMS.Application.Models
{
    public partial class ExerciseEvaluation
    {
        public Guid EvaluationId { get; set; }
        public Guid ClassEvaluationId { get; set; }
        public Guid StudentId { get; set; }
        public decimal EvaluationPoints { get; set; }

        public virtual ClassEvaluation ClassEvaluation { get; set; }
        public virtual Student Student { get; set; }
    }
}
