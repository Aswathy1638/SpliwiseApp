using Microsoft.AspNetCore.Mvc;
using SpliwiseApp.Interface;
using SpliwiseApp.Models;

namespace SpliwiseApp.Service
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        public ExpenseService(IExpenseRepository expenseRepository)
        {
           _expenseRepository = expenseRepository;
        }
        public async Task<ActionResult> CreateAsync(CreateExpense expense)
        {
            var result = await _expenseRepository.AddExpenseAsync(expense);
            await _expenseRepository.AddParticipants(expense.GroupId,expense.shareAmount);
            await _expenseRepository.AddToBalanceTable(expense);
           
            return new OkObjectResult(result);
        }

        public async Task<ActionResult> CreateTransactionAsync(CreateTransaction transaction)
        {
            var result = _expenseRepository.AddTransactionAsync(transaction);
            await _expenseRepository.UpdateBalanceTable(transaction.paidUserId,transaction.payerUserId,transaction.transaction_Amount);

            return new OkObjectResult(result);
        }

        public async Task<decimal> GetBalanceAync(string userId)
        {
            var balance = await _expenseRepository.GetBalance(userId);
            return balance;
        }

        public async Task<decimal> GetOweBalanceAsync(string userId)
        {
            var balance = await _expenseRepository.GetOweBalance(userId);
            return balance;
        }

        public async Task<decimal> GetOwedBalanceAsync(string userId)
        {
            var balance = await _expenseRepository.GetOwedBalance(userId);
            return balance;
        }

        public async Task<ActionResult<List<Expense>>> GetExpenseDetails(string userId)
        {
            var result = await _expenseRepository.GetExpenseDetails(userId); 

            return result;
        }
    }
}
