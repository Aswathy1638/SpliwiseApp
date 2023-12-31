﻿namespace SpliwiseApp.Models
{
//    public class User
//    {
//        public int id { get; set; }
//        public string name { get; set; }   
//        public string email { get; set; }
//        public string password { get; set; }

//        public ICollection<Group> Groups { get; set; }
//        public ICollection<Expense> Expenses { get; set; }
//        public ICollection<Transaction> Transactions { get; set; }
//        public ICollection<Balance> Balances { get; set; }
//    }
    public class UserLogin
{
    public string email { get; set; }
    public string password { get; set; }
}

public class UserRegister
{
  
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
}
    public class LoginResponseDto
    {
        public string Token { get; set; }

        public UserProfile Profile { get; set; }
    }

    public class UserProfile
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }

    }
}
