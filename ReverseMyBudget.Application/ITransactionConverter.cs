using ReverseMyBudget.Domain;
using System;

namespace ReverseMyBudget.Application
{
    public interface ITransactionConverter
    {
        Transaction Convert(Guid userId, Guid accountId, string line);
    }
}