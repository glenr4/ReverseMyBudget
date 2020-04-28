using ReverseMyBudget.Domain;
using ReverseMyBudget.Persistence.Sql;
using System;

namespace ReverseMyBudget.Application
{
    public interface ITransactionConverter
    {
        TransactionStaging Convert(Guid userId, Guid accountId, string line);
    }
}