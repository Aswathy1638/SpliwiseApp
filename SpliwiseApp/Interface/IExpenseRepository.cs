﻿using SpliwiseApp.Models;

namespace SpliwiseApp.Interface
{
    public interface IExpenseRepository
    {
        Task <Expense> AddExpenseAsync(CreateExpense expense);
        Task  AddParticipants(int groupId, decimal amount);
        Task AddToBalanceTable(CreateExpense expense);
        Task AddTransactionAsync(CreateTransaction transaction);
        Task UpdateBalanceTable(string paidUserId,string payerUserId,decimal amount);
    }
}
