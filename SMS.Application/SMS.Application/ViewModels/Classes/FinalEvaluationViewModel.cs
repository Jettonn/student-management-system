using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Classes
{
    public class FinalEvaluationViewModel
    {
        public Guid ClassId { get; set; }
        public int ClassAttendance { get; set; }
        public string ClassName { get; set; }
        public List<FinalEvaluationStudents> finalEvaluationStudents { get; set; }
    }

    public class FinalEvaluationStudents
    {
        public Guid StudentId { get; set; }
        public string Student { get; set; }
        public string Email { get; set; }
        public int Attendance { get; set; }
        public decimal TotalPoints { get; set; }
    }
}
