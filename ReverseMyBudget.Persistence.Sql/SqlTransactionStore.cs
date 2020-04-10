using Microsoft.EntityFrameworkCore;
using ReverseMyBudget.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public class SqlTransactionStore : ITransactionStore
    {
        private readonly ReverseMyBudgetDbContext _ctx;

        public SqlTransactionStore(ReverseMyBudgetDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<List<Transaction>> Get(Guid userId)
        {
            return _ctx.Transaction.Where(t => t.UserId == userId).ToListAsync();
        }

        public Task AddAsync(IEnumerable<Transaction> transactions)
        {
            _ctx.Transaction.AddRange(transactions);

            return _ctx.SaveChangesAsync();
        }
    }
}