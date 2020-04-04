using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ReverseMyBudget
{
    public static class ServicesExtensions
    {
        public static void AddOidcUserProvider(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.TryAddSingleton<IUserProvider, OidcUserProvider>();
        }
    }
}