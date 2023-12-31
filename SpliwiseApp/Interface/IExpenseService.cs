﻿using Microsoft.AspNetCore.Mvc;
using SpliwiseApp.Models;

namespace SpliwiseApp.Interface
{
    public interface IExpenseService
    {

        Task<ActionResult> CreateAsync(CreateExpense expense);
        Task<ActionResult> CreateTransactionAsync(CreateTransaction transaction);
        Task<decimal> GetBalanceAync(string userId);
        Task<decimal> GetOwedBalanceAsync(string userId);
        Task<decimal> GetOweBalanceAsync(string userId);
        Task<ActionResult<List<Expense>>> GetExpenseDetails(string userId);
        Task<ActionResult<BalOut>> GetBalanceDetails(string otherUser, string currentUser);

    }
}

