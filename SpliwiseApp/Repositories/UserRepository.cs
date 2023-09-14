using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpliwiseApp.Data;
using SpliwiseApp.Interface;
using SpliwiseApp.Models;
using System.Text.RegularExpressions;

namespace SpliwiseApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SplitContext _splitContext;
        public UserRepository(UserManager<IdentityUser> userManager, SplitContext splitContext)
        {
            _userManager = userManager;
            _splitContext = splitContext;
        }
        public Task<IdentityUser> FindByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        public Task<IdentityResult> CreateAsync(IdentityUser user,string Password) 
        {
            return _userManager.CreateAsync(user,Password);
        }
        public async Task<Models.Group> CreateGroupAsync(CreatGroup group, string email)
        {

            var newGroup = new Models.Group
            {
               
                CreatedDate = DateTime.Now,
                Name = group.Name,
                Description = group.Description,
            };
           await _splitContext.Groups.AddAsync(newGroup);
            
            await _splitContext.SaveChangesAsync();
            return (newGroup);
        }
        public async Task<Models.Group> FindByName(string name)
        {
            var existing = await _splitContext.Groups.FirstOrDefaultAsync(group => group.Name == name);

            if (existing == null)
            {
                return null;
            }
            return existing;

        }
        public async Task<Models.Group> AddUserToGroupAsync(string groupname, string email)
        {
            var group = await _splitContext.Groups.FirstOrDefaultAsync(g => g.Name == groupname);
            var user = await _userManager.FindByEmailAsync(email);
            
          

            var newUserGroup = new UserGroup
            {
                UserId = user.Id,
                GroupId = group.Id,
            };

            _splitContext.UserGroups.Add(newUserGroup);
           group.Users.Add(user);
            await _splitContext.SaveChangesAsync();

            return group;

        }
        public async Task<IEnumerable<UserProfile>> GetAllUsersAsync(int groupId)
        {
            var group = await _splitContext.Groups.
            Include(g => g.Users)
               .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
            {
                return null;
            }
            var usersIn = group.Users
                .Select(u => new UserProfile
                {
                    id = u.Id,
                    name = u.UserName,
                    email = u.Email
                });
            return usersIn.ToList();

        }

        public async Task<IEnumerable<Models.Group>> GetAllGroupsAsync(string userId)
        {
            var users = await _userManager.FindByIdAsync(userId);
            if (users == null)
            {
                return null;
            }
            var groups = _splitContext.UserGroups
               .Where(u =>u.UserId ==  userId)
               .Select(g =>g.GroupId) 
               .ToList();
             
            var groupsUser = await _splitContext.Groups.Where(g => groups.Contains(g.Id))
                .Select(u => new Models.Group
                {
                    Id = u.Id,
                    Name = u.Name,
                    Description = u.Description,
                    CreatedDate =u.CreatedDate,
                    
                })
                .ToListAsync();

            return  groupsUser;
        }

        public async Task<List<UserProfile>> GetFriendsAsync(string currentUserEmail)
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var users= allUsers.Where(u=>u.Email != currentUserEmail).Select(u=>new UserProfile
            {
                id = u.Id,
                name =u.UserName,
                email=u.Email
            }).ToList();
            return users;
        }
    }
}
