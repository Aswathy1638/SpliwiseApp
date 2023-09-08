using Microsoft.AspNetCore.Identity;
using SpliwiseApp.Models;

namespace SpliwiseApp.Interface
{
    public interface IUserRepository
    {
      Task<IdentityUser> FindByEmailAsync(string email);
        Task<IdentityResult> CreateAsync(IdentityUser user, string Password);
        Task <Group>FindByName(string name);
        Task<Group> CreateGroupAsync(CreatGroup group);
        Task<Group> AddUserToGroupAsync(string groupname, string email);
        Task<IEnumerable<UserProfile>> GetAllUsersAsync(int groupId);
        Task<IEnumerable<Group>> GetAllGroupsAsync(string userId);


    }

}
