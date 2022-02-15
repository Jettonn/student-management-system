using SMS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMS.Application.ViewModels.Lesson;
using SMS.Application.Models;
using SMS.Application.GenericRepository;
using SMS.Application.Enum;

namespace SMS.Application.Services
{
    public class LessonViewService : ILessonViewService
    {

        private readonly IGenericRepository<Lesson> lessonRepository;
        private readonly IGenericRepository<Attendance> attendanceRepository;
        private readonly IGenericRepository<Class> classesRepository;
        private readonly IGenericRepository<Subject> subjectRepository;
        private readonly IGenericRepository<Student> studentRepository;
        private readonly IGenericRepository<ClassStudent> classStudentsRepository;

        public LessonViewService(IGenericRepository<Lesson> lessonRepository,
                                IGenericRepository<Attendance> attendanceRepository,
                                IGenericRepository<Class> classesRepository,
                                IGenericRepository<Subject> subjectRepository,
                                IGenericRepository<Student> studentRepository,
                                IGenericRepository<ClassStudent> classStudentsRepository)
        {
            this.lessonRepository = lessonRepository;
            this.attendanceRepository = attendanceRepository;
            this.classesRepository = classesRepository;
            this.subjectRepository = subjectRepository;
            this.studentRepository = studentRepository;
            this.classStudentsRepository = classStudentsRepository;
        }

        public void EditStudentAttendance(Guid attendanceId, int attendanceStatus)
        {
            try
            {
                var attendance = attendanceRepository.GetSingleByCriteria(x => x.AttendanceId == attendanceId);
                var lesson = lessonRepository.GetSingleByCriteria(x => x.LessonId == attendance.LessonId);
                if (attendance.AttendaceStatus == attendanceStatus)
                {
                    attendance.AttendaceStatus = attendanceStatus;
                    attendanceRepository.Save();
                }
                else
                {
                    attendance.AttendaceStatus = attendanceStatus;
                    attendanceRepository.Save();
                    var classStudents = classStudentsRepository.GetSingleByCriteria(x => x.ClassId == lesson.ClassId && x.StudentId == attendance.StudentId);
                    if (attendanceStatus == (int)AttendanceType.Absent)
                    {
                        classStudents.StudentsHours -= lesson.Hours;
                    }
                    else
                    {
                        classStudents.StudentsHours += lesson.Hours;
                    }
                    classStudentsRepository.Save();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public string StartLesson(LessonViewModel model)
        {
            try
            {
                var id = Guid.NewGuid();
                var entity = new Lesson()
                {
                    LessonId = id,
                    ClassId = model.startLesson.ClassId,
                    Comment = model.startLesson.Comment,
                    Hours = model.startLesson.Hours,
                    Date = model.startLesson.Date
                };
                lessonRepository.Add(entity);

                var classInformation = classesRepository.GetById(model.startLesson.ClassId);
                var subjectInformation = subjectRepository.GetById(classInformation.SubjectId);
                var students = studentRepository.ListByCriteria(x => x.YearOfStudies == subjectInformation.YearOfStudies);

                if (students.Count() == 0)
                {
                    throw new ApplicationException("There are no students in the current year");
                }
                foreach (var student in students)
                {
                    var attendanceId = Guid.NewGuid();
                    var attendance = new Attendance()
                    {
                        AttendanceId = attendanceId,
                        LessonId = id,
                        StudentId = student.StudentId,
                        AttendaceStatus = (int)AttendanceType.Present
                    };
                    attendanceRepository.Add(attendance);
                }

                var classStudents = classStudentsRepository.ListByCriteria(x => x.ClassId == model.startLesson.ClassId);
                foreach (var classStudent in classStudents)
                {
                    var student = classStudentsRepository.GetById(classStudent.ClassStudentsId);
                    student.StudentsHours += model.startLesson.Hours;
                    classStudentsRepository.Save();
                }
                return id.ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        List<StudentAttendanceViewModel> ILessonViewService.GetAttendanceListByLessonId(Guid lessonId)
        {
            try
            {
                var attendanceList = new List<StudentAttendanceViewModel>();
                var attendances = attendanceRepository.ListByCriteria(x => x.LessonId == lessonId);
                foreach (var attendance in attendances)
                {
                    var student = studentRepository.GetById(attendance.StudentId);
                    attendanceList.Add(new StudentAttendanceViewModel
                    {
                        AttendanceId = attendance.AttendanceId,
                        AttendanceStatus = attendance.AttendaceStatus,
                        StudentName = student.Firstname + " " + student.Lastname
                    });
                }
                return attendanceList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
