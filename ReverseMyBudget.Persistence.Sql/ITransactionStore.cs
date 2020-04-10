using ReverseMyBudget.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public interface ITransactionStore
    {
        Task AddAsync(IEnumerable<Transaction> transactions);

        Task<PagedList<Transaction>> Get(Guid userId, TransactionQueryParameters parameters);
    }
}