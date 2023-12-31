﻿using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace SpliwiseApp.Models
{
    public class Group
    {
        public int  Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<IdentityUser> Users { get; set; } = new List<IdentityUser>();
        [JsonIgnore]
        public ICollection<Expense> Expenses { get; set; }
    }
    public class CreatGroup
    {

     
        public string Name { get; set; }
        public string Description { get; set; }
    }
  

}
