using Microsoft.AspNetCore.Identity;

namespace SpliwiseApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int groupId { get; set; }
        public string payerUserId { get; set; }
        public string paidUserId { get; set; }
        public int expenseId { get; set; }
        public decimal transaction_Amount { get; set; }
        public DateTime transaction_Date { get; set; }

        public Expense Expense { get; set; }
        public ICollection<IdentityUser> User { get; set; }




    }
    public class CreateTransaction
    {
        public int Id { get; set; }
        public int groupId { get; set; }
        public string payerUserId { get; set; }
        public string paidUserId { get; set; }
        public int expenseId { get; set; }
        public decimal transaction_Amount { get; set; }


    }
}
