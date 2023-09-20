using Microsoft.AspNetCore.Identity;

namespace SpliwiseApp.Models
{
    public class Balance
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string debtUserId { get; set; }
        public decimal balance_amount { get; set; }

        public IdentityUser user { get; set; }

    }


    public class BalOut
    {

        public decimal owe { get; set; }
        public decimal owed { get; set; }
    }
}
