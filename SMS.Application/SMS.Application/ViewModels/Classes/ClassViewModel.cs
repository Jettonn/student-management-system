using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Classes
{
    public class ClassViewModel
    {
        public string ClassName { get; set; }
        public string ClassComment { get; set; }
        public Guid Subject { get; set; }
        public int ClassHours { get; set; }
        public string ExamEvaluation { get; set; }
        public int ExamEvaluationPercentage { get; set; }
        public string AttendanceEvaluation { get; set; }
        public int AttendanceEvaluationPercentage { get; set; }
        public List<SelectListItem> Subjects { get; set; }
    }
}
