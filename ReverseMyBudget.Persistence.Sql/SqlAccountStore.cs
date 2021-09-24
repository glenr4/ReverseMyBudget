using Microsoft.EntityFrameworkCore;
using ReverseMyBudget.Domain;
using System.Collections.Generic;
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

        public async Task<List<Account>> GetAsync()
        {
            return await _ctx.Account.ToListAsync();
        }

        public Task AddAsync(Account account)
        {
            _ctx.Account.Add(account);

            return _ctx.SaveChangesAsync();
        }
    }
}