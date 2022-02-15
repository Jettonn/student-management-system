using SMS.Application.Enum;
using SMS.Application.GenericRepository;
using SMS.Application.Interfaces;
using SMS.Application.Models;
using SMS.Application.ViewModels.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Services
{
    public class ClassService : IClassService
    {
        private readonly IGenericRepository<Class> classRepository;
        private readonly IGenericRepository<ClassEvaluation> classEvaluationRepository;
        private readonly IGenericRepository<Subject> subjectsRepository;
        private readonly IGenericRepository<Student> studentsRepository;
        private readonly IGenericRepository<ExerciseEvaluation> exerciseEvaluationRepository;
        private readonly IGenericRepository<ClassStudent> classStudentsRepository;

        public ClassService(IGenericRepository<Class> classRepository,
                                IGenericRepository<ClassEvaluation> classEvaluationRepository,
                                IGenericRepository<Subject> subjectsRepository,
                                IGenericRepository<Student> studentsRepository,
                                IGenericRepository<ExerciseEvaluation> exerciseEvaluationRepository,
                                IGenericRepository<ClassStudent> classStudentsRepository)
        {
            this.classRepository = classRepository;
            this.classEvaluationRepository = classEvaluationRepository;
            this.subjectsRepository = subjectsRepository;
            this.studentsRepository = studentsRepository;
            this.exerciseEvaluationRepository = exerciseEvaluationRepository;
            this.classStudentsRepository = classStudentsRepository;
        }

        public string AddNewClass(ClassViewModel model, Guid professorId)
        {
            try
            {
                var id = Guid.NewGuid();
                var entity = new Class()
                {
                    ClassId = id,
                    ClassName = model.ClassName,
                    Comment = model.ClassComment,
                    PlannedHours = model.ClassHours,
                    ProfessorId = professorId,
                    SubjectId = model.Subject
                };
                classRepository.Add(entity);

                var exam = new ClassEvaluation()
                {
                    ClassId = id,
                    Name = "Exam",
                    Percentage = model.ExamEvaluationPercentage,
                    Type = (int)EvaluationType.Exam
                };
                classEvaluationRepository.Add(exam);

                var attendance = new ClassEvaluation()
                {
                    ClassId = id,
                    Name = "Attendance",
                    Percentage = model.AttendanceEvaluationPercentage,
                    Type = (int)EvaluationType.Attendace
                };
                classEvaluationRepository.Add(attendance);

                var subjctsInfo = subjectsRepository.GetById(model.Subject);
                var students = studentsRepository.ListByCriteria(x => x.YearOfStudies == subjctsInfo.YearOfStudies);
                foreach (var student in students)
                {
                    var newEvaluation = new ExerciseEvaluation()
                    {
                        ClassEvaluationId = exam.ClassEvaluationId,
                        StudentId = student.StudentId,
                        EvaluationPoints = 0
                    };
                    exerciseEvaluationRepository.Add(newEvaluation);

                    var newClassStudents = new ClassStudent()
                    {
                        ClassId = id,
                        StudentId = student.StudentId,
                        StudentsHours = 0
                    };
                    classStudentsRepository.Add(newClassStudents);
                }
                return id.ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void AddNewClassEvaluation(AddOtherEvaluations model)
        {
            try
            {
                var entity = new ClassEvaluation()
                {
                    ClassId = model.ClassId,
                    Name = model.Name,
                    Percentage = model.Percentage,
                    Type = (int)EvaluationType.Project
                };
                classEvaluationRepository.Add(entity);

                var classinfo = classRepository.GetById(model.ClassId);
                var subjctsInfo = subjectsRepository.GetById(classinfo.SubjectId);
                var students = studentsRepository.ListByCriteria(x => x.YearOfStudies == subjctsInfo.YearOfStudies);
                foreach (var student in students)
                {
                    var newEvaluation = new ExerciseEvaluation()
                    {
                        ClassEvaluationId = entity.ClassEvaluationId,
                        StudentId = student.StudentId,
                        EvaluationPoints = 0
                    };
                    exerciseEvaluationRepository.Add(newEvaluation);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void EditClass(EditClassDto model)
        {
            try
            {
                var classDto = classRepository.GetById(model.ClassId);
                classDto.Comment = model.ClassComment;
                classRepository.Save();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void EditEvaluation(EditEvaluationDto model)
        {
            try
            {
                var evaluation = classEvaluationRepository.GetById(model.ClassEvaluationId);
                evaluation.Name = model.Name;
                evaluation.Percentage = model.Percentage;
                classEvaluationRepository.Save();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void EditExerciseEvaluation(Guid evaluationId, decimal evaluationPercentage)
        {
            try
            {
                var exercise = exerciseEvaluationRepository.GetById(evaluationId);
                exercise.EvaluationPoints = evaluationPercentage;
                exerciseEvaluationRepository.Save();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
