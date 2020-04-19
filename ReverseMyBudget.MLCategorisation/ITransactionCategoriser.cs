using ReverseMyBudget.Domain;
using System.Collections.Generic;

namespace ReverseMyBudget.MLCategorisation
{
    public interface ITransactionCategoriser
    {
        IEnumerable<FullPrediction> CategoriseTransactions(IEnumerable<Transaction> transactions);
    }
}