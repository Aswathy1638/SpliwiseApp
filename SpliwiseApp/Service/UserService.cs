using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpliwiseApp.Interface;
using SpliwiseApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpliwiseApp.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<IdentityUser> userManager, IUserRepository userRepository,IConfiguration configuration)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _configuration = configuration;

        }

        public async Task<ActionResult> RegisterUserAsync(UserRegister user)
        {
            var existingUser = await _userRepository.FindByEmailAsync(user.email);
            if (existingUser != null)
            {
                return new ConflictObjectResult(new { message = "User alrready exists." });

            }

            var regUser = new IdentityUser
            { 
                UserName = user.name,
                Email = user.email,
            };
            var result = await _userRepository.CreateAsync(regUser, user.password);
            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(new { message = "User is already registered" });
            }
            
                var registerresult = new UserRegister
                {
                   
                    name = user.name,
                    email = user.email,
                    
                };
            return new JsonResult(registerresult);
        }

        public async Task<ActionResult<LoginResponseDto>> LoginUserAsync(UserLogin user)
        {
            var existingUser = await _userRepository.FindByEmailAsync(user.email);
            if (existingUser == null)
            {
                return new ConflictObjectResult(new { message = "User doesnot exists." });

            }
            var passwordCheck =await _userManager.CheckPasswordAsync(existingUser,user.password);
            if(!passwordCheck)
            {
                return new BadRequestObjectResult(new { message = "Password Mismatch" });
            }
            var token = GenerateJwtToken(existingUser.Id,existingUser.UserName,existingUser.Email);
           

            return new LoginResponseDto
            {
                Token = token,
                Profile = new UserProfile
                {
                    id = existingUser.Id,
                    name =existingUser.UserName,
                    email = existingUser.Email,
                }



            };


        }

      
        public async Task<ActionResult> CreateGroupAsync(CreatGroup group)
        {
           var groupName = await _userRepository.FindByName(group.Name);

            if (groupName!=null)
            {
                return new ConflictObjectResult(new { message = "The chosen group name already exists." });

            }
            var result = await _userRepository.CreateGroupAsync(group);
            return new OkObjectResult(result);

        }

        public async Task<List<UserProfile>> GetFriends(string currentUserEmail)
        {
            return await _userRepository.GetFriendsAsync(currentUserEmail);
        }

        public async Task<ActionResult<Group>> AddUserAsync(string groupname, string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            var group = await _userRepository.FindByName(groupname);

            if(user ==  null || group == null)
            {
                return new BadRequestObjectResult(new { message = "User or Group does not exist" });
            }
            if(group.Users.Any(u => u.Email == email))
            {
                return new BadRequestObjectResult(new { message = "User is already registerd with this group" });
            }
           

            var result = await _userRepository.AddUserToGroupAsync(groupname, email);
            return new OkObjectResult(result);

        }
        public async Task<ActionResult> GetAllUsersAsync(int groupId)
        {
            var result = await _userRepository.GetAllUsersAsync(groupId);
            return new OkObjectResult(result);
        }

        public async Task<ActionResult> GetAllGroupsAsync(string userId)
        {
            var result = await _userRepository.GetAllGroupsAsync(userId);
            return new OkObjectResult(result);
        }
        private string GenerateJwtToken(string id, string name, string email)
        {
            if (id == null || name == null || email == null)
            {
                throw new ArgumentNullException("name and email cannot be null or empty.");
            }
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: signin);


            string Token = new JwtSecurityTokenHandler().WriteToken(token);

            return Token;
        }
    }
}
