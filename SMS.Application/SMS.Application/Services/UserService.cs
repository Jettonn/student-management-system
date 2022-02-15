using SMS.Application.Extensions;
using SMS.Application.GenericRepository;
using SMS.Application.Interfaces;
using SMS.Application.Models;
using SMS.Application.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<Professor> professorRepository;
        public UserService(IGenericRepository<Professor> _professorRepository)
        {
            professorRepository = _professorRepository;
        }

        public void AddNewUser(AccountViewModel model)
        {
            try
            {
                var rsaEncryption = new RSAEncryption();
                var id = Guid.NewGuid();
                var entity = new Professor()
                {
                    ProfessorId = id,
                    Firstname = model.registerViewModel.Name,
                    Lastname = model.registerViewModel.Lastname,
                    Username = model.registerViewModel.Username,
                    Password = rsaEncryption.EncryptRSA(model.registerViewModel.Password),
                    DateCreated = DateTime.Now
                };
                professorRepository.Add(entity);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Professor Authenticate(string username, string password)
        {
            var user = professorRepository.GetSingleByCriteria(x => x.Username == username);

            if (user == null)
                return null;

            var rsaEncryption = new RSAEncryption();
            if (rsaEncryption.DecryptRSA(user.Password) != password)
                return null;

            return user;
        }

        public void ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var rsaEncryption = new RSAEncryption();
                var user = professorRepository.GetById(model.UserId);
                user.Password = rsaEncryption.EncryptRSA(model.ConfirmNewPassword);
                professorRepository.Save();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
