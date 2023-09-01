namespace SpliwiseApp.Models
{
    public class Balance
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int debtUserId { get; set; }
        public decimal balance_amount { get; set; }

        public User user { get; set; }

    }
}
