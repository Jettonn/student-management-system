using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.ViewModels.Classes
{
    public class EditClassViewModel
    {
        public EditClassDto editClassDto { get; set; }
        public AddOtherEvaluations addOtherEvaluations { get; set; }
        public EditEvaluationDto editEvaluationDto { get; set; }
    }

    public class EditClassDto
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassComment { get; set; }
        public Guid Subject { get; set; }
        public int ClassHours { get; set; }
        public int TotalPoints { get; set; }
        public List<SelectListItem> Subjects { get; set; }
        public List<EvaluationDto> EvaluationDtos { get; set; }
    }

    public class EvaluationDto
    {
        public Guid ClassEvaluationId { get; set; }
        public Guid ClassId { get; set; }
        public string Name { get; set; }
        public int Percentage { get; set; }
        public int Type { get; set; }
    }

    public class AddOtherEvaluations
    {
        public Guid ClassId { get; set; }
        public string Name { get; set; }
        public int Percentage { get; set; }
    }

    public class EditEvaluationDto
    {
        public Guid ClassEvaluationId { get; set; }
        public string Name { get; set; }
        public int Percentage { get; set; }
    }
}
