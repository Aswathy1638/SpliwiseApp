using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpliwiseApp.Models;

namespace SpliwiseApp.Interface
{
    public interface IUserService
    {
        Task<ActionResult> RegisterUserAsync(UserRegister user);
        Task<ActionResult<LoginResponseDto>> LoginUserAsync(UserLogin user);
        Task<ActionResult> CreateGroupAsync(CreatGroup group);
        Task <ActionResult<Group>>AddUserAsync(string groupname, string email);
        Task <ActionResult> GetAllUsersAsync(int groupId);
        Task<ActionResult> GetAllGroupsAsync(string userId);
    }
}