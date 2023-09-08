using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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


        //var newExpense = new Expense
        //{

        //    Description = expense.Description,
        //    GroupId = expense.GroupId,
        //    UserId = expense.UserId,
        //    paiduser_id = expense.paiduser_id,
        //    amount = expense.amount,
        //    shareAmount = expense.shareAmount,

        //};


        //var groupUsers = await _context.Groups
        //    .Include(g => g.Users)
        //    .FirstOrDefaultAsync(g => g.Id == newExpense.GroupId);
        //if (groupUsers == null)
        //{
        //    return NotFound(new { error = " Not found" });
        //}
        //foreach (var user in groupUsers.Users)
        //{
        //    var newParticipants = new Participants
        //    {
        //        GroupId = expense.GroupId,
        //        UserId = user.Id,
        //        amount = expense.shareAmount,
        //    };
        //    _context.Participants.Add(newParticipants);
        //    if (user.Id != newExpense.UserId)
        //    {
        //        var newBalance = new Balance
        //        {
        //            userId = user.Id,
        //            debtUserId = newExpense.UserId,
        //            balance_amount = expense.shareAmount

        //        };
        //        _context.Expenses.Add(newExpense);
        //        _context.Balances.Add(newBalance);
        //    }
        //    else
        //    {
        //        var newBalance = new Balance
        //        {

        //            userId = newExpense.UserId,
        //            debtUserId = newExpense.UserId,
        //            balance_amount = expense.shareAmount - expense.amount
        //        };
        //        _context.Expenses.Add(newExpense);
        //        _context.Balances.Add(newBalance);
        //    }



        //}
        //await _context.SaveChangesAsync();
        //}
        //[HttpPost("Transactions")]
        //public async Task<ActionResult<CreateTransaction>> MakeTransaction(CreateTransaction transaction)
        //{
        //    var newTransaction = new Transaction
        //    {
        //        groupId = transaction.groupId,
        //        payerUserId = transaction.payerUserId,
        //        paidUserId = transaction.paidUserId,
        //        expenseId = transaction.expenseId,
        //        transaction_Amount = transaction.transaction_Amount,
        //        transaction_Date = DateTime.Now,
        //    };
        //    _context.Transactions.Add(newTransaction);
        //    var paidUserBalance = _context.Balances.Where(u => u.userId == newTransaction.paidUserId);
        //    foreach (var user in paidUserBalance)
        //    {
        //        if (user.userId == newTransaction.paidUserId)
        //        {
        //            user.balance_amount -= user.balance_amount;
        //        }
        //        if (user.debtUserId == newTransaction.payerUserId)
        //        {
        //            user.balance_amount += user.balance_amount;
        //        }

        //    }
        //    if (paidUserBalance != null)
        //    {
        //        paidUserBalance.balance_amount -= transaction.transaction_Amount;
        //        _context.Balances.Update(paidUserBalance);

        //    }
        //    else
        //    {
        //        paidUserBalance.balance_amount = transaction.transaction_Amount;
        //        _context.Balances.Update(paidUserBalance);
        //    }

        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

    }
}
