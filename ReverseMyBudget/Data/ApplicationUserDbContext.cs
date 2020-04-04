using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReverseMyBudget.Models;

namespace ReverseMyBudget.Data
{
    /// <summary>
    /// This is for ApplicationUser Authentication only
    /// </summary>
    public class ApplicationUserDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationUserDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }
}