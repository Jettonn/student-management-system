using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Classes
{
    public class EvaluateProjectViewModel
    {
        public string ProjectName { get; set; }
        public decimal ProjectPoints { get; set; }
        public List<ExerciseEvaluationInfo> evaluationInfos { get; set; }
    }

    public class ExerciseEvaluationInfo
    {
        public Guid EvaluationId { get; set; }
        public Guid ClassEvaluationId { get; set; }
        public Guid StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public decimal EvaluationPoints { get; set; }
    }
}
