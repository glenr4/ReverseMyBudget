using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public interface ITransactionStagingStore
    {
        Task AddAsync(IEnumerable<TransactionStaging> transactions);
    }
}