using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Classes
{
    public class StudentEvaluations
    {
        public List<StudentEvaluationDto> studentEvaluationDtos { get; set; }
    }

    public class StudentEvaluationDto
    {
        public string EvaluationName { get; set; }
        public decimal EvaluationPoints { get; set; }
        public decimal EvaluationPercetange { get; set; }
    }
}
