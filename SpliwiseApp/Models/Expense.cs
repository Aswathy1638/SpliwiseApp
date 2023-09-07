﻿using Microsoft.AspNetCore.Identity;

namespace SpliwiseApp.Models
{
    public class Expense
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string GroupId { get; set; }
        public string UserId { get; set; }
        public string paiduser_id { get; set; }
        public decimal amount { get; set; }
        public decimal shareAmount { get; set; }

        public Group Group { get; set; }
        public ICollection<IdentityUser> Participants { get; set; }
        public ICollection<Transaction> Transactions { get; set; }

    }

    public class CreateExpense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public int paiduser_id { get; set; }
        public decimal amount { get; set; }
        public decimal shareAmount { get; set; }

    }
}
