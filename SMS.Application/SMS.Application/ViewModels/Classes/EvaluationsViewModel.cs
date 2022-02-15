using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Classes
{
    public class EvaluationsViewModel
    {
        public List<EvaluationInfo> evaluationInfo { get; set; }
    }

    public class EvaluationInfo
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public string Comment { get; set; }
        public int NumberOfProjects { get; set; }
        public string Color { get; set; }
        public string SubjectName { get; set; }
    }
}
