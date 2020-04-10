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
        public Task<List<Transaction>> Get([FromServices] ITransactionStore transactionStore)
        {
            return transactionStore.Get(_userProvider.UserId);
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