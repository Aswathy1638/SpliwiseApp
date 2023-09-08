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
    }
}
