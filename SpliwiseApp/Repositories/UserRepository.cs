using Microsoft.AspNetCore.Identity;
using SpliwiseApp.Interface;

namespace SpliwiseApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UserRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public Task<IdentityUser> FindByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        public Task<IdentityResult> CreateAsync(IdentityUser user,string Password) 
        {
            return _userManager.CreateAsync(user,Password);
        }
    }
}
