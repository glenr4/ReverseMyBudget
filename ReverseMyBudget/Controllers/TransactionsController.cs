using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReverseMyBudget.Application;
using ReverseMyBudget.Persistence;
using ReverseMyBudget.Persistence.Sql;
using System;
using System.Threading.Tasks;

namespace ReverseMyBudget.Controllers
{
    public class TransactionsController : AuthoriseControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] TransactionQueryParameters parameters,
            [FromServices] ITransactionStore transactionStore)
        {
            var result = await transactionStore.Get(parameters);

            return Ok(result);
        }

        [HttpPost("import/{accountId}")]
        public async Task<IActionResult> Create(Guid accountId, IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentException("Missing file");
            }

            // TODO: should the endpoint take responsibility for getting the UserId
            // and then the Mediator Request just gets it as a parameter or should
            // the Mediator Request get it?
            await _mediator.Send(new ImportTransactionsRequest
            {
                AccountId = accountId,
                FileName = file.FileName,
                File = file.OpenReadStream()
            });

            return Ok();
        }
    }
}