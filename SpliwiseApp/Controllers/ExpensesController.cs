using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpliwiseApp.Data;
using SpliwiseApp.Models;

namespace SpliwiseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly SplitContext _context;

        public ExpensesController(SplitContext context)
        {
            _context = context;
        }
       
        // POST: api/Expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Add Expenses ansd update the balance tavble
        [HttpPost]
        public async Task<ActionResult<CreateExpense>> PostExpense(CreateExpense expense)
        {
            if (_context.Expenses == null)
            {
                return Problem("Entity set 'SplitContext.Expenses'  is null.");
            }
            var newExpense = new Expense
            {
              
                Description = expense.Description,
                GroupId = expense.GroupId,
                UserId = expense.UserId,
                paiduser_id = expense.paiduser_id,
                amount = expense.amount,
                shareAmount = expense.shareAmount,

            };


            var groupUsers = await _context.Groups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == newExpense.GroupId);
            if (groupUsers == null)
            {
                return NotFound(new { error = " Not found" });
            }
            foreach (var user in groupUsers.Users)
            {
                var newParticipants = new Participants
                {
                    GroupId = expense.GroupId,
                    UserId = user.id,
                    amount = expense.shareAmount,
                };
                _context.Participants.Add(newParticipants);
                if (user.id != newExpense.UserId)
                {
                    var newBalance = new Balance
                    {
                        userId = user.id,
                        debtUserId = newExpense.UserId,
                        balance_amount = expense.shareAmount

                    };
                    _context.Expenses.Add(newExpense);
                    _context.Balances.Add(newBalance);
                }
                else
                {
                    var newBalance = new Balance
                    {
                  
                        userId = newExpense.UserId,
                        debtUserId = newExpense.UserId,
                        balance_amount = expense.shareAmount - expense.amount
                    };
                    _context.Expenses.Add(newExpense);
                    _context.Balances.Add(newBalance);
                }



            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("Transactions")]
        public async Task<ActionResult<CreateTransaction>> MakeTransaction(CreateTransaction transaction)
        {
            var newTransaction = new Transaction
            {
                groupId = transaction.groupId,
                payerUserId = transaction.payerUserId,
                paidUserId = transaction.paidUserId,
                expenseId = transaction.expenseId,
                transaction_Amount = transaction.transaction_Amount,
                transaction_Date =  DateTime.Now,
            };
            _context.Transactions.Add(newTransaction);
            var paidUserBalance = await _context.Balances.FindAsync(transaction.paidUserId);
            if (paidUserBalance != null)
            {
                paidUserBalance.balance_amount -= transaction.transaction_Amount;
                _context.Balances.Update(paidUserBalance);
               
            }
            else
            {
                paidUserBalance.balance_amount = transaction.transaction_Amount;
                _context.Balances.Update(paidUserBalance);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
