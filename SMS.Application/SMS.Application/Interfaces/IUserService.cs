using SMS.Application.Models;
using SMS.Application.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Application.Interfaces
{
    public interface IUserService
    {
        void AddNewUser(AccountViewModel model);
        Professor Authenticate(string username, string password);
        void ChangePassword(ChangePasswordViewModel model);
    }
}
