using System;

namespace ReverseMyBudget.Persistence
{
    public class TransactionQueryParameters : PagingQueryParameters
    {
        public string Description { get; set; }
        public string Test { get; set; }
        public DateRange DateLocal { get; set; } = new DateRange();
    }

    public class DateRange
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}