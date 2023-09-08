﻿using Microsoft.AspNetCore.Mvc;
using SpliwiseApp.Models;

namespace SpliwiseApp.Interface
{
    public interface IExpenseService
    {

        Task<ActionResult> CreateAsync(CreateExpense expense);

    }
}
