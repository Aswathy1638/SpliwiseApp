﻿using Microsoft.AspNetCore.Identity;

namespace SpliwiseApp.Models
{
    public class UserGroup
    {
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
