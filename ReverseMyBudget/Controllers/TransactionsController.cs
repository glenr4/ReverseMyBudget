using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReverseMyBudget.Application;
using ReverseMyBudget.Domain;
using ReverseMyBudget.Persistence;
using ReverseMyBudget.Persistence.Sql;
using Serilog;

namespace ReverseMyBudget.Controllers
{
    public class TransactionsController : AuthoriseControllerBase
    {
        private readonly IUserProvider _userProvider;
        private readonly IMediator _mediator;

        public TransactionsController(IUserProvider userProvider, IMediator mediator)
        {
            _userProvider = userProvider;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] TransactionQueryParameters parameters,
            [FromServices] ITransactionStore transactionStore)
        {
            var result = await transactionStore.Get(_userProvider.UserId, parameters);

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