using ReverseMyBudget.Domain;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public interface ITransactionStore
    {
        Task<PagedList<Transaction>> Get(TransactionQueryParameters parameters);
    }
}