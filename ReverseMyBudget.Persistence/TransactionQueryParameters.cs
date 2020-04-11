using System;

namespace ReverseMyBudget.Persistence
{
    /// <summary>
    /// Note: all parameter names must be the same as the type being filtered
    /// </summary>
    public class TransactionQueryParameters : PagingQueryParameters
    {
        public DateRange DateLocal { get; set; } = new DateRange();
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public Guid AccountId { get; set; }
        public Guid? CategoryId { get; set; }
        public bool IsDuplicate { get; set; }
    }
}