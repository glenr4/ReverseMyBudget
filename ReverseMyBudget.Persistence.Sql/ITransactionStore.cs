using ReverseMyBudget.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public interface ITransactionStore
    {
        Task<PagedList<Transaction>> GetAsync(TransactionQueryParameters parameters);
    }
}