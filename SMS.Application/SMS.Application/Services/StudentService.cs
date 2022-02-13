using SMS.Application.GenericRepository;
using SMS.Application.Interfaces;
using SMS.Application.Models;
using SMS.Application.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IGenericRepository<Student> studentRepository;

        public StudentService(IGenericRepository<Student> studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public void AddNewStudent(NewStudent model)
        {
            try
            {
                var id = Guid.NewGuid();
                var entity = new Student()
                {
                    StudentId = id,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    Gender = model.Gender,
                    YearOfStudies = model.YearOfStudies
                };
                studentRepository.Add(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void EditStudent(EditStudent model)
        {
            try
            {
                var entity = studentRepository.GetById(model.StudentId);
                entity.Firstname = model.Firstname;
                entity.Lastname = model.Lastname;
                entity.Email = model.Email;
                entity.Gender = model.Gender;
                entity.YearOfStudies = model.YearOfStudies;
                studentRepository.Save();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
