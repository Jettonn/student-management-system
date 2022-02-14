using SMS.Application.Models;
using SMS.Application.GenericRepository;
using SMS.Application.Interfaces;
using SMS.Application.ViewModels.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemiPerMenaxhiminEVijueshmerise.Services
{
    public class SubjectsViewService : ISubjectsViewService
    {
        private readonly IGenericRepository<Subject> subjectRepository;
        private readonly IGenericRepository<Professor> professorRepository;

        public SubjectsViewService(IGenericRepository<Subject> subjectRepository,
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

        public void EditSubject(EditSubjectViewModel model)
        {
            try
            {
                var entity = subjectRepository.GetById(model.SubjectId);
                entity.SubjectName = model.SubjectName;
                entity.SubjectType = model.SubjectType;
                entity.Comment = model.SubjectComment;
                entity.YearOfStudies = model.YearOfStudies;
                entity.Semester = model.Semester;
                entity.Color = model.Color;
                entity.NumberOfHours = model.NumberOfHours;
                subjectRepository.Save();
            }
            catch (Exception)
            {

                throw;
            };
        }
    }
}
