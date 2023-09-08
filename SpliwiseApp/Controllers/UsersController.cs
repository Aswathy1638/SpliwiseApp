using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using SpliwiseApp.Data;
using SpliwiseApp.Interface;
using SpliwiseApp.Models;

namespace SpliwiseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SplitContext _context;
        private readonly IUserService _userService;

        public UsersController(SplitContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }


        // To fetch all users in a group
        [HttpGet("groups/{groupId}/users")]
        public async Task<ActionResult<List<IdentityUser>>> GetUserinGroup(int groupId)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var users = await _userService.GetAllUsersAsync(groupId);

            return users;
        }

        //To fetch all the groups of a user

        [HttpGet("user/{userId}/groups")]
        public async Task<ActionResult<List<Group>>> GetGroupsofUser(string userId)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var groups = await _userService.GetAllGroupsAsync(userId);

            return groups;
        }
           
            // POST: api/Users
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            //Register User
            [HttpPost("register")]
        public async Task<ActionResult> PostUser(UserRegister user)
        {
            if (!ModelState.IsValid)
            {
                return Problem("Entity set 'SplitContext.Users'  is null.");
            }
            var registerResult = await _userService.RegisterUserAsync(user);
            return registerResult;
        }
        // Login User
        [HttpPost("login")]

        public async Task<ActionResult> Login(UserLogin user)
        {
            if (!ModelState.IsValid)
            {
                return Problem("Entity set 'SplitContext.Users'  is null.");
            }
            var loginResult = await _userService.LoginUserAsync(user);

            return Ok(new { loginResult.Value.Token, loginResult.Value.Profile });
        }

        [Authorize]
        [HttpPost("group")]
        public async Task<ActionResult<Group>> CreateGroup(CreatGroup group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.CreateGroupAsync(group);
            return result;

        }
        [Authorize]
        [HttpPost("group/{groupId}/users")]
        public async Task<ActionResult<Group>> AddToGroup(string groupname, string email)
        {
            if (groupname == null || email == null)
            { 
                return BadRequest(ModelState);
            }
            var result = await _userService.AddUserAsync(groupname, email);
            return Ok(new { message = " user added to the group" });

        }



    }


}
