using Microsoft.AspNetCore.Identity;

namespace SpliwiseApp.Models
{
    public class Group
    {
        public int  Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<IdentityUser> Users { get; set; } = new List<IdentityUser>();
        public ICollection<Expense> Expenses { get; set; }
    }
    public class CreatGroup
    {

     
        public string Name { get; set; }
        public string Description { get; set; }
    }
  

}
