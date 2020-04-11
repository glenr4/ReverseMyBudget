using System;

namespace ReverseMyBudget.Persistence
{
    public class TransactionQueryParameters : PagingQueryParameters
    {
        public string Description { get; set; }
        public DateTime? StartTimeLocal { get; set; }
        public DateTime? EndTimeLocal { get; set; }
    }
}