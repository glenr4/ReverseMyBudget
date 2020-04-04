using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ReverseMyBudget
{
    public static class UserServicesExtensions
    {
        public static void AddOidcUserProvider(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.TryAddScoped<IUserProvider, OidcUserProvider>();
        }
    }
}