using System;

namespace ReverseMyBudget
{
    public interface IUserProvider
    {
        public Guid UserId { get; }
    }
}