using Microsoft.AspNetCore.Mvc;
using SpliwiseApp.Models;

namespace SpliwiseApp.Interface
{
    public interface IExpenseRepository
    {
        Task<Expense> AddExpenseAsync(CreateExpense expense);
        Task AddParticipants(int groupId, decimal amount);
        Task AddToBalanceTable(CreateExpense expense);
        Task<Transaction> AddTransactionAsync(CreateTransaction transaction);
        Task UpdateBalanceTable(string paidUserId, string payerUserId, decimal amount);
        Task<decimal> GetBalance(string userId);
        Task<decimal> GetOwedBalance(string userId);
        Task<decimal> GetOweBalance(string userId);
        Task<ActionResult<List<Expense>>> GetExpenseDetails(string userId);
        Task<ActionResult<BalOut>> GetBalanceDetails(string otherUserId, string userId);
    }
}
