using ReverseMyBudget.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReverseMyBudget.Persistence.Sql
{
    public interface IAccountStore
    {
        Task AddAsync(Account account);
        Task<List<Account>> GetUsersAccountsAsync(Guid userId);
    }
}