using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SMS.Application.GenericRepository;
using SMS.Application.Interfaces;
using SMS.Application.Models;
using SMS.Application.ViewModels.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ISubjectsViewService subjectsViewService;
        private readonly IToastNotification toastNotification;
        private readonly IGenericRepository<Professor> professorRepository;
        private readonly IGenericRepository<Subject> subjectRepository;
        public IActionResult Index()
        {
            return View();
        }
        public SubjectsController(ISubjectsViewService subjectsViewService,
                            IToastNotification toastNotification,
                            IGenericRepository<Professor> professorRepository,
                            IGenericRepository<Subject> subjectRepository)
        {
            this.subjectsViewService = subjectsViewService;
            this.toastNotification = toastNotification;
            this.professorRepository = professorRepository;
            this.subjectRepository = subjectRepository;
        }

        public async Task<IActionResult> NewSubject(NewSubjectViewModel model)
        {
            try
            {
                var usernameLogged = HttpContext.User.Identity.Name;
                var currentUser = professorRepository.GetSingleByCriteria(x => x.Username == usernameLogged);
                if (!ModelState.IsValid)
                {
                    toastNotification.AddErrorToastMessage("An error occured, the subject couldn't be added!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                    return View(model);
                }
                subjectsViewService.AddSubject(model, currentUser.ProfessorId);
                ModelState.Clear();
                toastNotification.AddSuccessToastMessage("The subject added successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return View();
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("An error occured, the subject couldn't be added!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return View(model);
            }
        }
    }
}
