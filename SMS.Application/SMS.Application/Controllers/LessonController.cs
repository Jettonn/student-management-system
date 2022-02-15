using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SMS.Application.GenericRepository;
using SMS.Application.Interfaces;
using SMS.Application.Models;
using SMS.Application.ViewModels.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Controllers
{
    public class LessonController : Controller
    {
        private readonly IToastNotification toastNotification;
        private readonly IGenericRepository<Class> classRepository;
        private readonly IGenericRepository<Subject> subjctsRepository;
        private readonly ILessonViewService lessonViewService;
        private readonly IGenericRepository<Attendance> attendanceRepository;
        private readonly IGenericRepository<Lesson> lessonRepository;

        public LessonController(IToastNotification toastNotification,
                                IGenericRepository<Class> classRepository,
                                IGenericRepository<Subject> subjctsRepository,
                                ILessonViewService lessonViewService,
                                IGenericRepository<Attendance> attendanceRepository,
                                IGenericRepository<Lesson> lessonRepository)
        {
            this.toastNotification = toastNotification;
            this.classRepository = classRepository;
            this.subjctsRepository = subjctsRepository;
            this.lessonViewService = lessonViewService;
            this.attendanceRepository = attendanceRepository;
            this.lessonRepository = lessonRepository;
        }
        public IActionResult StartLesson()
        {
            var model = new LessonViewModel();
            List<LessonDto> lessons = new List<LessonDto>();
            var classes = classRepository.GetAll();
            foreach (var classData in classes)
            {
                var subject = subjctsRepository.GetSingleByCriteria(x => x.SubjectId == classData.SubjectId);
                var lessonsHeld = lessonRepository.ListByCriteria(x => x.ClassId == classData.ClassId);
                var hoursHeld = 0;
                foreach (var lesson in lessonsHeld)
                {
                    hoursHeld += lesson.Hours;
                }
                lessons.Add(new LessonDto
                {
                    ClassId = classData.ClassId,
                    ClassName = classData.ClassName,
                    ClassComment = classData.Comment,
                    PlannedHours = classData.PlannedHours - hoursHeld,
                    StudiesYear = subject.YearOfStudies,
                    SubjectColor = subject.Color,
                    SubjectName = subject.SubjectName,
                });
            }
            model.lessonDto = lessons.ToList();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(LessonViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    toastNotification.AddErrorToastMessage("An error occured, the lesson couldn't be started!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                    return View("StartLesson");
                }
                var lessonId = lessonViewService.StartLesson(model);
                toastNotification.AddSuccessToastMessage("The lesson started successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return RedirectToAction("OnlineLesson", new { lessonId = new Guid(lessonId) });
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("An error occured, the lesson couldn't be started!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return View("StartLesson");
            }
        }

        public IActionResult OnlineLesson(Guid lessonId)
        {
            var model = new OnlineLessonViewModel();
            var lesson = lessonRepository.GetById(lessonId);
            var classInfo = classRepository.GetById(lesson.ClassId);
            var subject = subjctsRepository.GetById(classInfo.SubjectId);
            var attendance = lessonViewService.GetAttendanceListByLessonId(lessonId);
            model.ClassName = classInfo.ClassName;
            model.ClassComment = classInfo.Comment;
            model.LessonDate = lesson.Date.ToString("dd/MM/yyyy");
            model.SubjectName = subject.SubjectName;
            model.ClassId = classInfo.ClassId;
            model.attendanceViewModels = attendance;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult EditStudentAttendance(Guid attendanceId, int attendanceStatus)
        {
            try
            {
                lessonViewService.EditStudentAttendance(attendanceId, attendanceStatus);
                toastNotification.AddSuccessToastMessage("The attendance updated successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(new { status = "Success" });
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("The attendance couldn't be updated successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(ex);
            }
        }
    }

}
