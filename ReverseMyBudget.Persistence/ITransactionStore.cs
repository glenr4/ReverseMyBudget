using ReverseMyBudget.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence
{
    public interface ITransactionStore
    {
        /// <summary>
        /// This should check for duplicates before inserting - how does the stored procedure currently do this?
        /// Perhaps I can make a uniqueness constraint?
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        Task AddAsync(IEnumerable<Transaction> transactions);
    }
}