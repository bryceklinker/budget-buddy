using System;

namespace BudgetBuddy.Api.General.Storage
{
    public interface IDocument
    {
        Guid Id { get; set; }
    }
}
