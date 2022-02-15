using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using SMS.Application.GenericRepository;
using SMS.Application.Interfaces;
using SMS.Application.Models;
using SMS.Application.ViewModels.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Controllers
{
    public class ClassesController : Controller
    {
        private readonly IGenericRepository<Subject> subjectRepository;
        private readonly IToastNotification toastNotification;
        private readonly IClassService classViewService;
        private readonly IGenericRepository<Professor> professorRepository;
        private readonly IGenericRepository<Class> classRepository;
        private readonly IGenericRepository<ClassEvaluation> classEvaluationRepository;

        public ClassesController(IToastNotification toastNotification,
                                    IGenericRepository<Subject> subjectRepository,
                                    IClassService classViewService,
                                    IGenericRepository<Professor> professorRepository,
                                    IGenericRepository<Class> classRepository,
                                    IGenericRepository<ClassEvaluation> classEvaluationRepository)
        {
            this.subjectRepository = subjectRepository;
            this.toastNotification = toastNotification;
            this.classViewService = classViewService;
            this.professorRepository = professorRepository;
            this.classRepository = classRepository;
            this.classEvaluationRepository = classEvaluationRepository;
        }

        [Authorize]
        public IActionResult NewClass(ClassViewModel model)
        {
            var subjects = subjectRepository.GetAll();
            List<SelectListItem> subjectsList = new List<SelectListItem>();
            foreach (var subject in subjects)
            {
                var id = subject.SubjectId.ToString();
                subjectsList.Add(new SelectListItem
                {
                    Value = id,
                    Text = subject.SubjectName
                });
            }
            model.Subjects = subjectsList.ToList();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClass(ClassViewModel model)
        {
            try
            {
                var usernameLogged = HttpContext.User.Identity.Name;
                var currentUser = professorRepository.GetSingleByCriteria(x => x.Username == usernameLogged);
                if (!ModelState.IsValid)
                {
                    toastNotification.AddErrorToastMessage("An error occured, the class couldn't be added!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                    return View(model);
                }
                var classId = classViewService.AddNewClass(model, currentUser.ProfessorId);
                toastNotification.AddSuccessToastMessage("The class added successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return RedirectToAction("EditClass", new { classId = new Guid(classId) });
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("An error occured, the class couldn't be added!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return View(model);
            }
        }

        [Authorize]
        public IActionResult ShowAllClasses()
        {
            var classes = classRepository.GetAll();
            return View(classes.OrderBy(x => x.ClassName).ToList());
        }

        [Authorize]
        public IActionResult EditClass(Guid classId)
        {
            try
            {
                var classDto = new EditClassDto();

                var classData = classRepository.GetById(classId);
                classDto.ClassId = classId;
                classDto.ClassName = classData.ClassName;
                classDto.ClassComment = classData.Comment;
                classDto.ClassHours = classData.PlannedHours;
                classDto.Subject = classData.SubjectId;

                var classEvaluations = classEvaluationRepository.ListByCriteria(x => x.ClassId == classId);
                List<EvaluationDto> evaluation = new List<EvaluationDto>();
                var sum = 0;
                foreach (var classEvaluation in classEvaluations)
                {
                    evaluation.Add(new EvaluationDto
                    {
                        ClassEvaluationId = classEvaluation.ClassEvaluationId,
                        ClassId = classEvaluation.ClassId,
                        Name = classEvaluation.Name,
                        Percentage = Convert.ToInt32(classEvaluation.Percentage),
                        Type = classEvaluation.Type,
                    });
                    sum += Convert.ToInt32(classEvaluation.Percentage);
                }
                var evaluationsOrdered = evaluation.OrderBy(x => x.Type).ToList();
                classDto.EvaluationDtos = evaluationsOrdered;
                classDto.TotalPoints = sum;

                var subjects = subjectRepository.GetAll();
                List<SelectListItem> subjectsList = new List<SelectListItem>();
                foreach (var subject in subjects)
                {
                    var id = subject.SubjectId.ToString();
                    subjectsList.Add(new SelectListItem
                    {
                        Value = id,
                        Text = subject.SubjectName
                    });
                }
                classDto.Subjects = subjectsList.ToList();

                var model = new EditClassViewModel();
                model.editClassDto = classDto;
                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClass(EditClassViewModel model)
        {
            try
            {
                var editDto = new EditClassDto();
                editDto = model.editClassDto;
                classViewService.EditClass(editDto);
                toastNotification.AddSuccessToastMessage("Class updated successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return RedirectToAction("EditClass", new { classId = model.editClassDto.ClassId });
            }
            catch (Exception)
            {
                toastNotification.AddErrorToastMessage("An error occured, the class couldn't be updated!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return RedirectToAction("EditClass", new { classId = model.editClassDto.ClassId });
            }
        }
    }
}
