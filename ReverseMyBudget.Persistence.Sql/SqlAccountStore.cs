using Microsoft.EntityFrameworkCore;
using ReverseMyBudget.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public class SqlAccountStore : IAccountStore
    {
        private readonly ReverseMyBudgetDbContext _ctx;

        public SqlAccountStore(ReverseMyBudgetDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<Account>> GetUsersAccountsAsync(Guid userId)
        {
            return await _ctx.Account.Where(a => a.UserId == userId).ToListAsync();
        }

        public Task AddAsync(Account account)
        {
            _ctx.Account.Add(account);

            return _ctx.SaveChangesAsync();
        }
    }
}