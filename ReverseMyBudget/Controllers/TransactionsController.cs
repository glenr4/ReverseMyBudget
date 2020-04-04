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
using Serilog;

namespace ReverseMyBudget.Controllers
{
    public class TransactionsController : AuthoriseControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public TransactionsController(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Remove AllowAnonymous after testing
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("import/{accountId}")]
        public async Task<IActionResult> Create(Guid accountId, IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentException("Missing file");
            }

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