using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReverseMyBudget.Domain;
using ReverseMyBudget.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReverseMyBudget.Controllers
{
    public class AccountsController : AuthoriseControllerBase
    {
        private readonly IAccountStore _accountStore;

        public AccountsController(IAccountStore accountStore)
        {
            _accountStore = accountStore;
        }

        [HttpGet]
        public Task<List<Account>> Get()
        {
            return _accountStore.GetAsync();
        }

        /// <summary>
        /// TODO: Remove AllowAnonymous and enable UserId update when there is a UI for creating accounts
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public Task Add(Account account)
        {
            //account.UserId = _userProvider.UserId;

            return _accountStore.AddAsync(account);
        }
    }
}