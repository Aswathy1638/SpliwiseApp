using Microsoft.AspNetCore.Identity;

namespace SpliwiseApp.Models
{
    public class Participants
    {
        public  string id { get; set; }
        public string GroupId { get; set; }
        public string UserId { get; set; }
        public decimal amount { get; set; }

    }
    public class ExpenseParticipant
    {
        public string ExpenseId { get; set; }
        public Expense Expense { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }

}
