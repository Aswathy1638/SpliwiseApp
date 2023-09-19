using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpliwiseApp.Models;

namespace SpliwiseApp.Interface
{
    public interface IUserService
    {
        Task<ActionResult> RegisterUserAsync(UserRegister user);
        Task<ActionResult<LoginResponseDto>> LoginUserAsync(UserLogin user);
        Task<ActionResult> CreateGroupAsync(CreatGroup group,string email);
        Task <ActionResult<Group>>AddUserAsync(string groupname, List<string> email);
        Task <ActionResult> GetAllUsersAsync(int groupId);
        Task<ActionResult> GetAllGroupsAsync(string userId);
        Task<List<UserProfile>> GetFriends(string currentUserEmail);
        Task<ActionResult<FriendShip>> AddFriend(string email,string current);
        Task<ActionResult> GetMyFriendsAsync(string currentUser);
    }
}