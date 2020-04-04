using ReverseMyBudget.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence
{
    public interface ITransactionStore
    {
        Task AddAsync(IEnumerable<Transaction> transactions);
    }
}