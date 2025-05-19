using FinalAuthProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAuthProject.Services
{
    public interface IAuthService
    {
        Task<User> Authenticate(LoginModel login);
        string GenerateJwtToken(User user);

    }
}