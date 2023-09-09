using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpliwiseApp.Data;
using SpliwiseApp.Interface;
using SpliwiseApp.Models;

namespace SpliwiseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly SplitContext _context;
        private readonly IExpenseService _expenseService;

        public ExpensesController(SplitContext context, IExpenseService expenseService)
        {
            _context = context;
            _expenseService = expenseService;
        }

       // POST: api/Expenses

       [HttpPost("Expense")]
        public async Task<ActionResult<CreateExpense>> PostExpense(CreateExpense expense)
        {
            if (_context.Expenses == null)
            {
                return Problem("Entity set 'SplitContext.Expenses'  is null.");
            }
            var createExpense = await _expenseService.CreateAsync(expense);
            return createExpense;

        }

        [HttpPost("Transaction")]
        public async Task<ActionResult<CreateTransaction>> MakeTransaction(CreateTransaction transaction)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'SplitContext.Expenses'  is null.");
            }
            var createTransaction = await _expenseService.CreateTransactionAsync(transaction);

            return createTransaction;
        }

        [HttpGet("Expense/Balance")]
        public async Task<decimal> GetBalance(string userId)
        {
            var userBalnce = await _expenseService.GetBalanceAync(userId);
            return userBalnce;
        }


        [HttpGet("Expense/Balance/owe")]
        public async Task<decimal> GetOweBalance(string userId)
        {
            var userBalnce = await _expenseService.GetOweBalanceAsync(userId);
            return userBalnce;
        }

        [HttpGet("Expense/Balance/Owed")]
        public async Task<decimal> GetUserinGroup(string userId)
        {
            var userBalnce = await _expenseService.GetOwedBalanceAsync(userId);
            return userBalnce;
        }


    }
}
