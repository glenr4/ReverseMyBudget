using System;

namespace ReverseMyBudget.Persistence
{
    public class TransactionQueryParameters : PagingQueryParameters
    {
        public string Description { get; set; }
        public string Test { get; set; }
        public DateRange DateLocal { get; set; } = new DateRange();
        //public DateTime? StartTimeLocal { get; set; }
        //public DateTime? EndTimeLocal { get; set; }
    }

    public class DateRange
    {
        public DateTime? StartDate { get; set; } = new DateTime(2020, 03, 16);
        public DateTime? EndDate { get; set; }
    }
}