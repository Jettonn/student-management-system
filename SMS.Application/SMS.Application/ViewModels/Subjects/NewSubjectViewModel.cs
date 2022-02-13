using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Subjects
{
    public class NewSubjectViewModel
    {
        [Required]
        public string SubjectName { get; set; }
        public string SubjectComment { get; set; }
        [Required]
        public int SubjectType { get; set; }
        [Required]
        public int YearOfStudies { get; set; }
        [Required]
        public int Semester { get; set; }
        [Required]
        public int NumberOfHours { get; set; }
        public string Color { get; set; }
    }
}
