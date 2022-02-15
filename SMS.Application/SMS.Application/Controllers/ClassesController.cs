using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using SMS.Application.Enum;
using SMS.Application.GenericRepository;
using SMS.Application.Interfaces;
using SMS.Application.Models;
using SMS.Application.ViewModels.Classes;
using SMS.Application.ViewModels.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IGenericRepository<ExerciseEvaluation> exerciseEvaluationRepository;
        private readonly IGenericRepository<Student> studentsRepository;
        private readonly IGenericRepository<Lesson> lessonRepository;
        private readonly IGenericRepository<Attendance> attendanceRepository;
        private readonly ILessonViewService lessonViewService;
        private readonly IGenericRepository<ClassStudent> classStudentsRepository;

        public ClassesController(IToastNotification toastNotification,
                                    IGenericRepository<Subject> subjectRepository,
                                    IClassService classViewService,
                                    IGenericRepository<Professor> professorRepository,
                                    IGenericRepository<Class> classRepository,
                                    IGenericRepository<ClassEvaluation> classEvaluationRepository,
                                    IGenericRepository<ExerciseEvaluation> exerciseEvaluationRepository,
                                    IGenericRepository<Student> studentsRepository,
                                    IGenericRepository<Lesson> lessonRepository,
                                    IGenericRepository<Attendance> attendanceRepository,
                                    ILessonViewService lessonViewService,
                                    IGenericRepository<ClassStudent> classStudentsRepository)
        {
            this.subjectRepository = subjectRepository;
            this.toastNotification = toastNotification;
            this.classViewService = classViewService;
            this.professorRepository = professorRepository;
            this.classRepository = classRepository;
            this.classEvaluationRepository = classEvaluationRepository;
            this.exerciseEvaluationRepository = exerciseEvaluationRepository;
            this.studentsRepository = studentsRepository;
            this.lessonRepository = lessonRepository;
            this.attendanceRepository = attendanceRepository;
            this.lessonViewService = lessonViewService;
            this.classStudentsRepository = classStudentsRepository;
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

        [Authorize]
        [HttpPost]
        public IActionResult AddNewEvaluation(EditClassViewModel model)
        {
            try
            {
                classViewService.AddNewClassEvaluation(model.addOtherEvaluations);
                toastNotification.AddSuccessToastMessage("New evaluation added successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return RedirectToAction("EditClass", new { classId = model.addOtherEvaluations.ClassId });
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("New evaluation couldn't be added successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return RedirectToAction("EditClass", new { classId = model.addOtherEvaluations.ClassId });
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteEvaluation(Guid evaluationId)
        {
            try
            {
                classEvaluationRepository.Delete(evaluationId);
                toastNotification.AddSuccessToastMessage("The evaluation was deleted successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(new { status = "Success" });
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("The evaluation couldn't delete successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(ex);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditEvaluation(Guid EvaluationId, string Name, int Percentage)
        {
            try
            {
                var model = new EditEvaluationDto
                {
                    ClassEvaluationId = EvaluationId,
                    Name = Name,
                    Percentage = Percentage
                };
                if (!ModelState.IsValid)
                {
                    throw new Exception("Data is not valid!");
                }
                classViewService.EditEvaluation(model);
                toastNotification.AddSuccessToastMessage("The evaluation updated successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(new { status = "Success" });
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("An error occured, the evaluation couldn't be updated!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(ex);
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteClass(Guid classId)
        {
            try
            {
                classRepository.Delete(classId);
                toastNotification.AddSuccessToastMessage("The class was deleted successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(new { status = "Success" });
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("The class couldn't delete successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(ex);
            }
        }

        [Authorize]
        public IActionResult Evaluations()
        {
            var model = new EvaluationsViewModel();
            List<EvaluationInfo> evaluations = new List<EvaluationInfo>();
            var classes = classRepository.GetAll();
            foreach (var classData in classes)
            {
                var subjectData = subjectRepository.GetById(classData.SubjectId);
                var numberOfProjects = classEvaluationRepository.ListByCriteria(x => x.ClassId == classData.ClassId && x.Type == (int)EvaluationType.Project).Count();
                var evaluation = new EvaluationInfo
                {
                    ClassId = classData.ClassId,
                    ClassName = classData.ClassName,
                    Color = subjectData.Color,
                    Comment = classData.Comment,
                    NumberOfProjects = numberOfProjects,
                    SubjectName = subjectData.SubjectName
                };
                evaluations.Add(evaluation);
            }
            model.evaluationInfo = evaluations.OrderBy(x => x.ClassName).ToList();
            return View(model);
        }

        [Authorize]
        public IActionResult EvaluateProject(Guid classEvaluationId)
        {
            var model = new EvaluateProjectViewModel();
            var classEvaluation = classEvaluationRepository.GetById(classEvaluationId);
            var exerciseEvaluations = exerciseEvaluationRepository.ListByCriteria(x => x.ClassEvaluationId == classEvaluationId);
            List<ExerciseEvaluationInfo> ExerciseEvaluationInfo = new List<ExerciseEvaluationInfo>();
            foreach (var exerciseInfo in exerciseEvaluations)
            {
                var student = studentsRepository.GetById(exerciseInfo.StudentId);
                var exerciseEvaluation = new ExerciseEvaluationInfo
                {
                    ClassEvaluationId = exerciseInfo.ClassEvaluationId,
                    EvaluationId = exerciseInfo.EvaluationId,
                    StudentId = student.StudentId,
                    FullName = student.Firstname + " " + student.Lastname,
                    Email = student.Email,
                    EvaluationPoints = exerciseInfo.EvaluationPoints
                };
                ExerciseEvaluationInfo.Add(exerciseEvaluation);
            }
            model.evaluationInfos = ExerciseEvaluationInfo.OrderBy(x => x.FullName).ToList();
            model.ProjectName = classEvaluation.Name;
            model.ProjectPoints = classEvaluation.Percentage;
            return View(model);
        }

        [HttpGet]
        public ActionResult GetEvaluations(Guid classId)
        {
            var classEvaluations = classEvaluationRepository.ListByCriteria(x => x.ClassId == classId && x.Type != (int)EvaluationType.Attendace);
            List<SelectListItem> evaluationList = new List<SelectListItem>();
            foreach (var evaluation in classEvaluations)
            {
                var id = evaluation.ClassEvaluationId.ToString();
                evaluationList.Add(new SelectListItem
                {
                    Value = id,
                    Text = evaluation.Name
                });
            }
            return Json(evaluationList);
        }

        [Authorize]
        [HttpPost]
        public JsonResult EditExerciseEvaluation(Guid evaluationId, decimal evaluationPercentage)
        {
            try
            {
                classViewService.EditExerciseEvaluation(evaluationId, evaluationPercentage);
                //toastNotification.AddSuccessToastMessage("The attendance updated successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(new { status = "Success" });
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("The evaluation couldn't be updated successfully.", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return Json(ex);
            }
        }

        [Authorize]
        public IActionResult Attendance(Guid classId)
        {
            var model = new AttendanceViewModel();
            var lessons = lessonRepository.ListByCriteria(x => x.ClassId == classId);
            List<AttendanceDto> attendanceDto = new List<AttendanceDto>();
            foreach (var lesson in lessons)
            {
                var absent = attendanceRepository.ListByCriteria(x => x.AttendaceStatus == (int)AttendanceType.Absent && x.LessonId == lesson.LessonId).Count();
                var present = attendanceRepository.ListByCriteria(x => x.AttendaceStatus == (int)AttendanceType.Present && x.LessonId == lesson.LessonId).Count();
                var attendanceInfo = new AttendanceDto
                {
                    LessonId = lesson.LessonId,
                    Comment = lesson.Comment,
                    Hours = lesson.Hours,
                    Date = lesson.Date.ToString("dd/MM/yyyy HH:mm"),
                    Absent = absent,
                    Present = present
                };
                attendanceDto.Add(attendanceInfo);
            }
            var attendances = attendanceDto.OrderByDescending(x => x.Date).ToList();
            model.attendanceDtos = attendances.OrderBy(x => x.Date).ToList();
            return View(model);
        }

        [Authorize]
        public IActionResult EditAttendance(Guid lessonId)
        {
            var model = new OnlineLessonViewModel();
            var lesson = lessonRepository.GetById(lessonId);
            var classInfo = classRepository.GetById(lesson.ClassId);
            var subject = subjectRepository.GetById(classInfo.SubjectId);
            var attendance = lessonViewService.GetAttendanceListByLessonId(lessonId).OrderBy(x => x.StudentName).ToList();
            model.ClassName = classInfo.ClassName;
            model.ClassComment = classInfo.Comment;
            model.LessonDate = lesson.Date.ToString("dd/MM/yyyy");
            model.SubjectName = subject.SubjectName;
            model.ClassId = classInfo.ClassId;
            model.attendanceViewModels = attendance;
            return View(model);
        }

        [Authorize]
        public IActionResult FinalEvaluations(Guid classId)
        {
            var model = new FinalEvaluationViewModel();
            List<FinalEvaluationStudents> studentsEvaluation = new List<FinalEvaluationStudents>();
            var classInfo = classRepository.GetById(classId);
            var subject = subjectRepository.GetById(classInfo.SubjectId);

            var students = studentsRepository.ListByCriteria(x => x.YearOfStudies == subject.YearOfStudies);
            foreach (var student in students)
            {
                decimal projectsEvaluationPoints = 0;
                var classEvaluations = classEvaluationRepository.ListByCriteria(x => x.ClassId == classInfo.ClassId && x.Type != (int)EvaluationType.Attendace);
                foreach (var evaluation in classEvaluations)
                {
                    var evaluationPoints = exerciseEvaluationRepository.GetSingleByCriteria(x => x.ClassEvaluationId == evaluation.ClassEvaluationId && x.StudentId == student.StudentId);
                    projectsEvaluationPoints += evaluationPoints != null ? evaluationPoints.EvaluationPoints : 0;
                }

                var attendance = classStudentsRepository.GetSingleByCriteria(x => x.ClassId == classInfo.ClassId && x.StudentId == student.StudentId);
                var attendancePercentage = classEvaluationRepository.GetSingleByCriteria(x => x.ClassId == classInfo.ClassId && x.Type == (int)EvaluationType.Attendace);
                decimal attendancePoints = ((Convert.ToDecimal(attendance != null ? attendance.StudentsHours : 0) / Convert.ToDecimal(classInfo.PlannedHours)) * Convert.ToDecimal(attendancePercentage.Percentage));
                var studentsInfo = new FinalEvaluationStudents
                {
                    StudentId = student.StudentId,
                    Student = student.Firstname + " " + student.Lastname,
                    Email = student.Email,
                    Attendance = attendance != null ? attendance.StudentsHours : 0,
                    TotalPoints = decimal.Round(projectsEvaluationPoints, 2) + decimal.Round(attendancePoints, 2)
                };
                studentsEvaluation.Add(studentsInfo);
            }
            model.ClassName = classInfo.ClassName;
            model.ClassId = classInfo.ClassId;
            model.ClassAttendance = classInfo.PlannedHours;
            model.finalEvaluationStudents = studentsEvaluation.OrderBy(x => x.Student).ToList();
            return View(model);
        }

        public IActionResult FinalEvaluationCsv(Guid classId)
        {
            List<FinalEvaluationStudents> studentsEvaluation = new List<FinalEvaluationStudents>();
            var classInfo = classRepository.GetById(classId);
            var subject = subjectRepository.GetById(classInfo.SubjectId);

            var students = studentsRepository.ListByCriteria(x => x.YearOfStudies == subject.YearOfStudies);
            foreach (var student in students)
            {
                decimal projectsEvaluationPoints = 0;
                var classEvaluations = classEvaluationRepository.ListByCriteria(x => x.ClassId == classInfo.ClassId && x.Type != (int)EvaluationType.Attendace);
                foreach (var evaluation in classEvaluations)
                {
                    var evaluationPoints = exerciseEvaluationRepository.GetSingleByCriteria(x => x.ClassEvaluationId == evaluation.ClassEvaluationId && x.StudentId == student.StudentId);
                    projectsEvaluationPoints += evaluationPoints != null ? evaluationPoints.EvaluationPoints : 0;
                }

                var attendance = classStudentsRepository.GetSingleByCriteria(x => x.ClassId == classInfo.ClassId && x.StudentId == student.StudentId);
                var attendancePercentage = classEvaluationRepository.GetSingleByCriteria(x => x.ClassId == classInfo.ClassId && x.Type == (int)EvaluationType.Attendace);
                decimal attendancePoints = ((Convert.ToDecimal(attendance != null ? attendance.StudentsHours : 0) / Convert.ToDecimal(classInfo.PlannedHours)) * Convert.ToDecimal(attendancePercentage.Percentage));
                var studentsInfo = new FinalEvaluationStudents
                {
                    StudentId = student.StudentId,
                    Student = student.Firstname + " " + student.Lastname,
                    Email = student.Email,
                    Attendance = attendance != null ? attendance.StudentsHours : 0,
                    TotalPoints = decimal.Round(projectsEvaluationPoints, 2) + decimal.Round(attendancePoints, 2)
                };
                studentsEvaluation.Add(studentsInfo);
            }
            var builder = new StringBuilder();
            builder.AppendLine("Student,Email,Attendance,Total Points");
            foreach (var studentEvaluation in studentsEvaluation)
            {
                builder.AppendLine($"{studentEvaluation.Student},{studentEvaluation.Email},{studentEvaluation.Attendance} of {classInfo.PlannedHours},{studentEvaluation.TotalPoints}");
            }

            var title = classInfo.ClassName + " - Final Evaluation.csv";
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", title);
        }

        [HttpGet]
        public ActionResult GetStudentEvaluations(Guid classId, Guid studentId)
        {
            var model = new StudentEvaluations();
            var classEvaluations = classEvaluationRepository.ListByCriteria(x => x.ClassId == classId && x.Type != (int)EvaluationType.Attendace);
            List<StudentEvaluationDto> evaluationList = new List<StudentEvaluationDto>();
            foreach (var evaluation in classEvaluations)
            {
                var studentEvaluation = exerciseEvaluationRepository.GetSingleByCriteria(x => x.ClassEvaluationId == evaluation.ClassEvaluationId && x.StudentId == studentId);
                evaluationList.Add(new StudentEvaluationDto
                {
                    EvaluationName = evaluation.Name,
                    EvaluationPercetange = evaluation.Percentage,
                    EvaluationPoints = studentEvaluation != null ? studentEvaluation.EvaluationPoints : 0
                });
            }
            model.studentEvaluationDtos = evaluationList;
            return Json(model);
        }
    }
}
