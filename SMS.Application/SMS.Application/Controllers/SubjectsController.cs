using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet]
        public IActionResult ShowAllSubjects()
        {
            var subjects = subjectRepository.GetAll();
            return View(subjects.OrderBy(x => x.SubjectName).ToList());
        }

        [Authorize]
        public IActionResult EditSubject(Guid subjectId)
        {
            var model = new EditSubjectViewModel();
            var subject = subjectRepository.GetById(subjectId);
            model.SubjectId = subject.SubjectId;
            model.SubjectName = subject.SubjectName;
            model.SubjectComment = subject.Comment;
            model.SubjectType = subject.SubjectType;
            model.YearOfStudies = subject.YearOfStudies;
            model.Semester = subject.Semester;
            model.NumberOfHours = subject.NumberOfHours;
            model.Color = subject.Color;
            return View(model);
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditSubject(EditSubjectViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    toastNotification.AddErrorToastMessage("An error occured, the subject couldn't be updated!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                    return View(model);
                }
                subjectsViewService.EditSubject(model);
                toastNotification.AddSuccessToastMessage("The subject updated successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return RedirectToAction("ShowAllSubjects");
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("An error occured, the subject couldn't be updated!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return View(model);
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteSubject(Guid subjectId)
        {
            try
            {
                subjectRepository.Delete(subjectId);
                toastNotification.AddSuccessToastMessage("The subject was deleted successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(new { status = "Success" });
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("The subject couldn't delete successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(ex);
            }
        }
    }
}
