﻿using System;
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
        //[HttpGet("usersinGroup{groupId}")]
        //public async Task<ActionResult<List<IdentityUser>>> GetUserinGroup(int groupId)
        //{
        //    if (_context.Users == null)
        //    {
        //        return NotFound();
        //    }
        //    var group = await _context.Groups.
        //        Include(g => g.Users)
        //        .FirstOrDefaultAsync(g => g.Id == groupId);

        //    if (group == null)
        //    {
        //        return NotFound();
        //    }
        //    var usersIn = group.Users
        //        .Select(u => new User
        //        {
        //            id = u.id,
        //            name = u.name,
        //            email = u.email
        //        });
        //    return Ok(usersIn);
        //}

        //To fetch all the groups of a user

        //[HttpGet("groupsofUser{userId}")]
        //public async Task<ActionResult<List<Group>>> GetGroupsofUser(int userId)
        //{
        //    if (_context.Users == null)
        //    {
        //        return NotFound();
        //    }
        //    var user = await _context.Users.
        //        Include(g => g.Groups)
        //        .FirstOrDefaultAsync(g => g.id == userId);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    var groupsIn = user.Groups
        //        .Select(u => new Group
        //        {
        //            Id = u.Id,
        //            Name = u.Name,
        //            Description = u.Description
        //        });
        //    return Ok(groupsIn);
        //}


        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    if (id != user.id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

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

            return Ok(new { loginResult.Value.Token,loginResult.Value.Profile});
        }

        [Authorize]
        [HttpPost("creategroup")]
        public async Task<ActionResult<Group>> CreateGroup(CreatGroup group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.CreateGroupAsync(group);
            return result;

        }
        //[HttpPost("addUserToGroup")]
        //public async Task<ActionResult<Group>> AddToGroup(int groupId, int userId)
        //{
        //    if (groupId <= 0 || userId <= 0)
        //    { return BadRequest(ModelState); }
        //    var searchGroup = _context.Groups.SingleOrDefault(g => g.Id == groupId);
        //    var searchUser = _context.Users.SingleOrDefault(u => u.id == userId);
        //    if (searchGroup == null || searchUser == null)
        //    {
        //        return BadRequest(new { error = "user or group not found" });
        //    }
        //    if (searchGroup.Users.Any(u => u.id == userId))
        //    {
        //        return BadRequest(new { error = "User is already in this group" });
        //    }
        //    searchGroup.Users.Add(searchUser);
        //    await _context.SaveChangesAsync();
        //    return Ok(new { message = " user added to the group" });

        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    if (_context.Users == null)
        //    {
        //        return NotFound();
        //    }
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool UserExists(int id)
        //{
        //    return (_context.Users?.Any(e => e.id == id)).GetValueOrDefault();
        //}
    }


}
