using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpliwiseApp.Data;
using SpliwiseApp.Interface;
using SpliwiseApp.Models;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace SpliwiseApp.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        public readonly SplitContext _splitContext;

        public ExpenseRepository(SplitContext splitContext)
        {
            _splitContext = splitContext;
        }

        public async Task<Expense> AddExpenseAsync(CreateExpense expense)
        {

            var newExpense = new Expense
            {
                Description = expense.Description,
                GroupId = expense.GroupId,
                UserId = expense.UserId,
                paiduser_id = expense.paiduser_id,
                amount = expense.amount,
                shareAmount = expense.shareAmount,

            };

            await _splitContext.Expenses.AddAsync(newExpense);
            await UpdateBalanceTable(expense.UserId, expense.paiduser_id, expense.amount);
            await _splitContext.SaveChangesAsync();
            return newExpense;

        }
        public async Task AddParticipants(int groupId, decimal amount)
        {
            var group = await _splitContext.Groups.
                         Include(g => g.Users)
                        .FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
            {
                throw new ArgumentException("Invalid group ID");

            }

            foreach (var user in group.Users)
            {
                var newParticipant = new Participants
                {
                    GroupId = groupId,
                    UserId = user.Id,
                    amount = amount
                };
                await _splitContext.Participants.AddAsync(newParticipant);
            }
            await _splitContext.SaveChangesAsync();


        }
        public async Task AddToBalanceTable(CreateExpense expense)
        {
            var group = await _splitContext.Groups.
                         Include(g => g.Users)
                        .FirstOrDefaultAsync(g => g.Id == expense.GroupId);
            if (group == null)
            {
                throw new ArgumentException("Invalid group ID");
            }
            foreach (var user in group.Users)
            {
                if (user.Id != expense.UserId)
                {
                    var newBalance = new Balance
                    {
                        userId = user.Id,
                        debtUserId = expense.UserId,
                        balance_amount = expense.shareAmount

                    };

                    _splitContext.Balances.Add(newBalance);
                    
                    await _splitContext.SaveChangesAsync();

                }
                else
                {
                    var newBalance = new Balance
                    {

                        userId = expense.UserId,
                        debtUserId = expense.UserId,
                        balance_amount = expense.shareAmount - expense.amount
                    };
                    _splitContext.Balances.Add(newBalance);

                    await _splitContext.SaveChangesAsync();
                }

            }

        }
        public async Task<Transaction> AddTransactionAsync(CreateTransaction transaction)
        {
            var newTransaction = new Transaction
            {
                groupId = transaction.groupId,
                payerUserId = transaction.payerUserId,
                paidUserId = transaction.paidUserId,
                expenseId = transaction.expenseId,
                transaction_Amount = transaction.transaction_Amount,
                transaction_Date = DateTime.Now,
            };
            await _splitContext.Transactions.AddAsync(newTransaction);
            await UpdateBalanceTable(transaction.paidUserId, transaction.payerUserId, transaction.transaction_Amount);
            await _splitContext.SaveChangesAsync();
            return newTransaction;

        }

        public async Task UpdateBalanceTable(string paidUserId, string payerUserId, decimal transaction_Amount)
        {
            var paidUserBalance = await _splitContext.Balances.FirstOrDefaultAsync(u => u.userId == paidUserId);
            var payerUserBalance = await _splitContext.Balances.FirstOrDefaultAsync(u => u.userId == payerUserId);

            if (paidUserBalance != null && payerUserBalance != null)
            {
                paidUserBalance.balance_amount -= transaction_Amount;
                payerUserBalance.balance_amount += transaction_Amount;

                _splitContext.Balances.Update(paidUserBalance);
                _splitContext.Balances.Update(payerUserBalance);

                await _splitContext.SaveChangesAsync();
            }
        }
        public async Task<decimal> GetBalance(string userId)
        {
            var userBalance =  _splitContext.Balances.Where(u => u.userId == userId || u.debtUserId == userId);
            decimal balAmount = 0;
            foreach(var balance in userBalance) 
            {
                balAmount += balance.balance_amount;
            }
            return balAmount;
        }

        public async Task<decimal> GetOwedBalance(string userId)
        {
            var userBalance = _splitContext.Balances.Where(u => u.userId == userId || u.debtUserId == userId);
            decimal balAmount = 0;
            foreach (var balance in userBalance)
            {
                if(balance.balance_amount < 0)
                {

                balAmount += balance.balance_amount;
                }
            }
            return balAmount;
        }
        public async Task<decimal> GetOweBalance(string userId)
        {
            var userBalance = _splitContext.Balances.Where(u => u.userId == userId || u.debtUserId == userId);
            decimal balAmount = 0;
            foreach (var balance in userBalance)
            {
                if (balance.balance_amount > 0)
                {

                    balAmount += balance.balance_amount;
                }
            }
            return balAmount;
        }

        public async Task<ActionResult<List<Expense>>> GetExpenseDetails(string userId)
        {
           
           var result = await _splitContext.Expenses
        .Where(e => e.UserId == userId || e.paiduser_id == userId)
        .ToListAsync();
           
               
            return result;
        }

    }
}
    

