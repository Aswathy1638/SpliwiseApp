using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public ICollection<IdentityUser> User { get; set; }




    }
    public class CreateTransaction
    {
      
        public int groupId { get; set; }
        public string payerUserId { get; set; }
        public string paidUserId { get; set; }
        public int expenseId { get; set; }
        public decimal transaction_Amount { get; set; }


    }
}
