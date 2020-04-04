using ReverseMyBudget.Domain;
using Serilog.Core;
using Serilog.Events;

namespace ReverseMyBudget
{
    public class UserLogEnricher : ILogEventEnricher
    {
        private readonly IUserProvider _userProvider;

        public UserLogEnricher(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(IUserProvider.UserId), _userProvider.UserId));
        }
    }
}