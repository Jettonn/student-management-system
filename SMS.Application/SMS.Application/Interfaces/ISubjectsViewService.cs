using SMS.Application.ViewModels.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Interfaces
{
    public interface ISubjectsViewService
    {
        public void AddSubject(NewSubjectViewModel model, Guid professorId);
        public void EditSubject(EditSubjectViewModel model);

    }
}
