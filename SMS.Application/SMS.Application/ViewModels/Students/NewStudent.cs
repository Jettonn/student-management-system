using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Students
{
    public class NewStudent
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int YearOfStudies { get; set; }
    }
}
