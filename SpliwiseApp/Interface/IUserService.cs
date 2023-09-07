using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpliwiseApp.Models;

namespace SpliwiseApp.Interface
{
    public interface IUserService
    {
        Task<ActionResult> RegisterUserAsync(UserRegister user);
        Task<ActionResult<LoginResponseDto>> LoginUserAsync(UserLogin user);
    }
}