namespace SpliwiseApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int groupId { get; set; }
        public int payerUserId { get; set; }
        public int paidUserId { get; set; }
        public int expenseId { get; set; }
        public decimal transaction_Amount { get; set; }
        public DateTime transaction_Date { get; set; }

        public Expense Expense { get; set; }
        public ICollection<User> User { get; set; }




    }
}
