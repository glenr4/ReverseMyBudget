using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReverseMyBudget.Domain;

namespace ReverseMyBudget
{
    public static class TestUserServiceExtensions
    {
        public static void AddTestUserProvider(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.TryAddScoped<IUserProvider, TestUserProvider>();
        }
    }
}