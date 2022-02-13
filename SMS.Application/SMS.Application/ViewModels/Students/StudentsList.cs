using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Students
{
    public class StudentsList
    {
        public StudentsList(StudentsDto studentsDto)
        {
            this.studentsDto = studentsDto;
        }
        public StudentsDto studentsDto { get; set; }
    }

    public class StudentsDto
    {
        public Guid StudentId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public int YearOfStudies { get; set; }
    }
}
