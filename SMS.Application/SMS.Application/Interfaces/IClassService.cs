using SMS.Application.ViewModels.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Interfaces
{
    public interface IClassService
    {
        public string AddNewClass(ClassViewModel model, Guid professorId);
        public void AddNewClassEvaluation(AddOtherEvaluations model);
        public void EditEvaluation(EditEvaluationDto model);
        public void EditClass(EditClassDto model);
        public void EditExerciseEvaluation(Guid evaluationId, decimal evaluationPercentage);
    }
}
