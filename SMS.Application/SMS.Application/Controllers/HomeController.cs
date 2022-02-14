using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using SMS.Application.Enum;
using SMS.Application.GenericRepository;
using SMS.Application.Models;
using SMS.Application.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IToastNotification _toastNotification;
        private readonly IGenericRepository<Student> studentRepository;
        private readonly IGenericRepository<Subject> subjectRepository;
        private readonly IGenericRepository<Class> classesRepository;
        private readonly IGenericRepository<ClassEvaluation> classEvaluationRepository;

        public HomeController(ILogger<HomeController> logger,
                                IToastNotification toastNotification,
                                IGenericRepository<Student> studentRepository,
                                IGenericRepository<Subject> subjectRepository,
                                IGenericRepository<Class> classesRepository,
                                IGenericRepository<ClassEvaluation> classEvaluationRepository)
        {
            _logger = logger;
            _toastNotification = toastNotification;
            this.studentRepository = studentRepository;
            this.subjectRepository = subjectRepository;
            this.classesRepository = classesRepository;
            this.classEvaluationRepository = classEvaluationRepository;
        }
        public IActionResult Index()
        {
            var model = new HomeViewModel();
            var students = studentRepository.GetAll().Count();
            var studentsFemale = studentRepository.ListByCriteria(x => x.Gender == (int)Gender.Female).Count();
            var studentsMale = studentRepository.ListByCriteria(x => x.Gender == (int)Gender.Male).Count();
            var studentsFirst = studentRepository.ListByCriteria(x => x.YearOfStudies == (int)YearOfStudies.One).Count();
            var studentsSecond = studentRepository.ListByCriteria(x => x.YearOfStudies == (int)YearOfStudies.Two).Count();
            var studentsThird = studentRepository.ListByCriteria(x => x.YearOfStudies == (int)YearOfStudies.Three).Count();
            var subjects = subjectRepository.GetAll().Count();
            var classes = classesRepository.GetAll().Count();
            var projects = classEvaluationRepository.ListByCriteria(x => x.Type == (int)EvaluationType.Project).Count();
            var classesFirst = classesRepository.ListByCriteria(x => x.Subject.YearOfStudies == (int)YearOfStudies.One).Count();
            var classesSecond = classesRepository.ListByCriteria(x => x.Subject.YearOfStudies == (int)YearOfStudies.Two).Count();
            var classesThird = classesRepository.ListByCriteria(x => x.Subject.YearOfStudies == (int)YearOfStudies.Three).Count();
            model.Students = students;
            model.StudentsFemale = studentsFemale;
            model.StudentsMale = studentsMale;
            model.StudentsFirstYear = studentsFirst;
            model.StudentsSecondYear = studentsSecond;
            model.StudentsThirdYear = studentsThird;
            model.Subjects = subjects;
            model.Classes = classes;
            model.Projects = projects;
            model.ClassesFirstYear = classesFirst;
            model.ClassesSecondYear = classesSecond;
            model.ClassesThirdYear = classesThird;
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
