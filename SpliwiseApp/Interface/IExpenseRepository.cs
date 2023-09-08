using SpliwiseApp.Models;

namespace SpliwiseApp.Interface
{
    public interface IExpenseRepository
    {
        Task <Expense> AddExpenseAsync(CreateExpense expense);
        Task  AddParticipants(int groupId, decimal amount);
        Task AddToBalanceTable(CreateExpense expense);
    }
}
