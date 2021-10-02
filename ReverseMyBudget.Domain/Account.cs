using System;

namespace ReverseMyBudget.Domain
{
    public class Account
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}