using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public interface IAddDistinctToTransactions
    {
        Task<int> Execute();
    }
}