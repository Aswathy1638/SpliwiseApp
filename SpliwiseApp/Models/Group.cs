namespace SpliwiseApp.Models
{
    public class Group
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public DateTime CreatedDate { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
