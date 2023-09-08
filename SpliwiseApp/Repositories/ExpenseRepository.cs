using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpliwiseApp.Data;
using SpliwiseApp.Interface;
using SpliwiseApp.Models;
using System.Text.RegularExpressions;

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
    }
}
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