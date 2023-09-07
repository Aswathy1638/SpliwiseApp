using Microsoft.AspNetCore.Identity;

namespace SpliwiseApp.Interface
{
    public interface IUserRepository
    {
      Task<IdentityUser> FindByEmailAsync(string email);
        Task<IdentityResult> CreateAsync(IdentityUser user, string Password);


    }

}
