using System;

namespace ReverseMyBudget.Domain
{
    public interface IUserProvider
    {
        Guid UserId { get; }
    }
}