using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SMS.Application.GenericRepository;
using SMS.Application.Interfaces;
using SMS.Application.Models;
using SMS.Application.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IGenericRepository<Student> studentRepository;
        private readonly IToastNotification toastNotification;
        private readonly IStudentService studentService;

        public StudentsController(IGenericRepository<Student> studentRepository,
                                    IToastNotification toastNotification,
                                    IStudentService studentService)
        {
            this.studentRepository = studentRepository;
            this.toastNotification = toastNotification;
            this.studentService = studentService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult StudentsList()
        {
            var students = studentRepository.GetAll();
            var studentsOrdered = students.OrderBy(x => x.Firstname).ThenBy(x => x.Lastname).ToList();
            return View(studentsOrdered);
        }

        [Authorize]
        public IActionResult NewStudent()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewStudent(NewStudent model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    toastNotification.AddErrorToastMessage("An error occured, the student couldn't be added!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                    return View(model);
                }
                var students = studentRepository.GetAll();
                if (students.Where(x => x.Firstname == model.Firstname && x.Lastname == model.Lastname && x.Email == model.Email).Any())
                {
                    toastNotification.AddWarningToastMessage("There is a student with this email address!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                    return View(model);
                }
                studentService.AddNewStudent(model);
                ModelState.Clear();
                toastNotification.AddSuccessToastMessage("The student added successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return View("NewStudent");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteStudent(Guid studentId)
        {
            try
            {
                studentRepository.Delete(studentId);
                toastNotification.AddSuccessToastMessage("The student was deleted successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(new { status = "Success" });
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("The student couldn't delete successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(ex);
            }
        }

        [Authorize]
        public IActionResult EditStudent(Guid studentId)
        {
            var model = new EditStudent();
            var student = studentRepository.GetById(studentId);
            model.StudentId = student.StudentId;
            model.Firstname = student.Firstname;
            model.Lastname = student.Lastname;
            model.Email = student.Email;
            model.Gender = student.Gender;
            model.YearOfStudies = student.YearOfStudies;
            return View(model);
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditStudent(EditStudent model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    toastNotification.AddErrorToastMessage("An error occured, the student couldn't be updated!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                    return View(model);
                }
                studentService.EditStudent(model);
                toastNotification.AddSuccessToastMessage("The student updated successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return RedirectToAction("StudentsList");

        }
    }
}
