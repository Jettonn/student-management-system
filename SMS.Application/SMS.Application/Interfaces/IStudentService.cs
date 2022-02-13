using SMS.Application.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Interfaces
{
    public interface IStudentService
    {
        void AddNewStudent(NewStudent model);
        void EditStudent(EditStudent model);
    }
}
