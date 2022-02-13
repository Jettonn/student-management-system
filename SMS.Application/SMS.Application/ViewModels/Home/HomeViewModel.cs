using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Home
{
    public class HomeViewModel
    {
        public int Students { get; set; }
        public int Subjects { get; set; }
        public int Classes { get; set; }
        public int Projects { get; set; }
        public int StudentsFemale { get; set; }
        public int StudentsMale { get; set; }
        public int StudentsFirstYear { get; set; }
        public int StudentsSecondYear { get; set; }
        public int StudentsThirdYear { get; set; }
        public int ClassesFirstYear { get; set; }
        public int ClassesSecondYear { get; set; }
        public int ClassesThirdYear { get; set; }
    }
}
