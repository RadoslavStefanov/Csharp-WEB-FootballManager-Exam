using FootballManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Contracts
{
    public interface IUserService
    {
        (bool registered, IEnumerable<ErrorViewModel> errors) Register(RegisterViewModel model);
        string Login(LoginViewModel model);
        string? GetUsername(string userId);
        (string userId, bool isCorrect) IsLoginCorrect(LoginViewModel model);
    }
}
