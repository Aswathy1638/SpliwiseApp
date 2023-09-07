using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpliwiseApp.Data;
using SpliwiseApp.Interface;
using SpliwiseApp.Models;

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
        public Task<Group> CreateGroupAsync(CreatGroup group)
        {
            var newGroup = new Group
            {
               
                CreatedDate = DateTime.Now,
                Name = group.Name,
                Description = group.Description,
            };
            _splitContext.Groups.Add(newGroup);
            _splitContext.SaveChangesAsync();
            return Task.FromResult(newGroup);
        }
        public async Task<Group> FindByName(string name)
        {
            var existing = await _splitContext.Groups.FirstOrDefaultAsync(group => group.Name == name);

            if (existing == null)
            {
                return null;
            }
            return existing;

        }
    }
}
