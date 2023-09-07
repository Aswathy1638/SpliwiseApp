using Microsoft.AspNetCore.Identity;

namespace SpliwiseApp.Models
{
    public class Participants
    {
        public  int id { get; set; }
        public int GroupId { get; set; }
        public string UserId { get; set; }
        public decimal amount { get; set; }

    }
    public class ExpenseParticipant
    {
        public int ExpenseId { get; set; }
        public Expense Expense { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }

}
