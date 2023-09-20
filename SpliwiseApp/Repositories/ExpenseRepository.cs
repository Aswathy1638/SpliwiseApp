using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpliwiseApp.Data;
using SpliwiseApp.Interface;
using SpliwiseApp.Models;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace SpliwiseApp.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        public readonly SplitContext _splitContext;
        private readonly UserManager<IdentityUser> _userManager;

        public ExpenseRepository(SplitContext splitContext, UserManager<IdentityUser> userManager)
        {
            _splitContext = splitContext;
            
            _userManager = userManager;
        }

        public async Task<Expense> AddExpenseAsync(CreateExpense expense)
        {
            var group = await  _splitContext.Groups.FirstOrDefaultAsync(g => g.Id == expense.GroupId);
            //var paidUesrId = await _userManager.FindByNameAsync(expense.PaidUserName);

            if (group == null)
            {
                return null;
            }
        
            decimal totalMem = await _splitContext.UserGroups.Where(g => g.GroupId == group.Id).CountAsync();
            decimal share = expense.amount / totalMem;

            var newExpense = new Expense
            {
                Description = expense.Description,
                GroupId = expense.GroupId,
                UserId = expense.UserId,
                paiduser_id =expense.PaidUserId,
                amount = expense.amount,
                shareAmount = share,

            };

            await _splitContext.Expenses.AddAsync(newExpense);
            await UpdateBalanceTable(expense.PaidUserId,expense.UserId , expense.amount);
            await AddParticipants(group.Id, share);
            await AddToBalanceTable(expense);
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

            var groupspec = await _splitContext.Groups.FirstOrDefaultAsync(g => g.Id == expense.GroupId);

            var group = await _splitContext.Groups.
                         Include(g => g.Users)
                        .FirstOrDefaultAsync(g => g.Id == groupspec.Id);
            decimal totalMem = await _splitContext.UserGroups.Where(g => g.GroupId == group.Id).CountAsync();
            decimal share = expense.amount / totalMem;

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
                        balance_amount = share

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
                        balance_amount = share - expense.amount
                    };
                    _splitContext.Balances.Add(newBalance);

                    await _splitContext.SaveChangesAsync();
                }

            }

        }
        public async Task<Transaction> AddTransactionAsync(CreateTransaction transaction)
        {
            var groupId = await _splitContext.Groups.FirstOrDefaultAsync(u => u.Id == transaction.GroupId);
            //var user = await _userManager.FindByNameAsync(transaction.payerName);
            var newTransaction = new Transaction
            {
                groupId = groupId.Id,
                payerUserId = transaction.paidUserId,
                paidUserId = transaction.payerName,
                expenseId = transaction.expenseId,
                transaction_Amount = transaction.transaction_Amount,
                transaction_Date = DateTime.Now,
            };
            await _splitContext.Transactions.AddAsync(newTransaction);
            await UpdateBalanceTable(transaction.payerName, transaction.paidUserId, transaction.transaction_Amount);
            await _splitContext.SaveChangesAsync();
            return newTransaction;

        }

        public async Task UpdateBalanceTable(string payerUserId, string paidUserId, decimal transaction_Amount)
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
                if (balance.balance_amount > 0 && balance.debtUserId != userId)
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


        public async Task<ActionResult<BalOut>> GetBalanceDetails(string otherUserId, string userId)
        {
            decimal owe = 0;
            decimal owed = 0;
        var iotherEmail = await _userManager.FindByNameAsync(userId);
            string Oid = iotherEmail.Id;
            var userBalance = _splitContext.Balances.Where(u => u.userId == Oid  || u.debtUserId ==otherUserId);
            
            foreach(var balance in userBalance)
            {
                if(balance.balance_amount > 0 )
                {
                    owe += balance.balance_amount;
                }
              
            }
            var userBal2= _splitContext.Balances.Where(u => u.userId == otherUserId || u.debtUserId == Oid);
            foreach (var balance in userBalance)
            {
                if (balance.balance_amount < 0)
                {
                    owed += balance.balance_amount;
                }

            }

            var newResult = new BalOut { 
            owe = owe,
            owed = owed,
            };
            
            return newResult;
        }

    }
}
    

