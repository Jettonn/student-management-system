using SMS.Application.GenericRepository;
using SMS.Application.Interfaces;
using SMS.Application.Models;
using SMS.Application.ViewModels.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Services
{
    public class SubjectsService : ISubjectsViewService
    {
        private readonly IGenericRepository<Subject> subjectRepository;
        private readonly IGenericRepository<Professor> professorRepository;

        public SubjectsService(IGenericRepository<Subject> subjectRepository,
                                    IGenericRepository<Professor> professorRepository)
        {
            this.subjectRepository = subjectRepository;
            this.professorRepository = professorRepository;
        }

        public void AddSubject(NewSubjectViewModel model, Guid professorId)
        {
            try
            {
                var id = Guid.NewGuid();
                var entity = new Subject()
                {
                    SubjectId = id,
                    SubjectName = model.SubjectName,
                    Comment = model.SubjectComment,
                    SubjectType = model.SubjectType,
                    ProfessorId = professorId,
                    YearOfStudies = model.YearOfStudies,
                    Semester = model.Semester,
                    NumberOfHours = model.NumberOfHours,
                    Color = model.Color
                };
                subjectRepository.Add(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
