﻿using System.ComponentModel.DataAnnotations;

namespace SpliwiseApp.Models
{
    public class Friends
    {

        public int Id { get; set; }
        public string FriendId { get; set; }
        public string UserId { get; set; }
        public string FriendName { get; set; }
        public string UserName { get; set; }


    }
}
