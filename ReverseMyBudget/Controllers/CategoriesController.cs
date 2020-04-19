using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReverseMyBudget.Domain;
using ReverseMyBudget.MLCategorisation;
using ReverseMyBudget.Persistence;
using ReverseMyBudget.Persistence.Sql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReverseMyBudget.Controllers
{
    public class CategoriesController : AuthoriseControllerBase
    {
        [AllowAnonymous]
        [HttpPost("auto")]
        public async Task<IActionResult> AutoCategorise([FromServices] ITransactionStore transactionStore, [FromServices] ITransactionCategoriser transactionCategoriser)
        {
            TransactionQueryParameters parameters = new TransactionQueryParameters();

            PagedList<Transaction> transactions = await transactionStore.GetAsync(parameters);

            var result = transactionCategoriser.CategoriseTransactions(transactions.Adapt<List<Transaction>>());

            return Ok(result);
        }
    }
}