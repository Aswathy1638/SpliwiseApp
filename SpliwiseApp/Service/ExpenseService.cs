﻿using Microsoft.AspNetCore.Mvc;
using SpliwiseApp.Interface;
using SpliwiseApp.Models;
using SpliwiseApp.Repositories;

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
      
           
            return new OkObjectResult(result);
        }

        public async Task<ActionResult> CreateTransactionAsync(CreateTransaction transaction)
        {
            var result = await  _expenseRepository.AddTransactionAsync(transaction);
          

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

        public async Task<ActionResult<BalOut>> GetBalanceDetails(string otherUser, string currentUser)
        {
            var result = await _expenseRepository.GetBalanceDetails(otherUser, currentUser);
            return new OkObjectResult(result);

        }
    }
}
