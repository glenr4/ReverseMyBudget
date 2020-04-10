using System;

namespace ReverseMyBudget.Domain
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        /// <summary>
        /// Imported data is assumed to be in the user's timezone
        /// </summary>
        public DateTime DateLocal { get; set; }

        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal? Balance { get; set; }
        public Guid AccountId { get; set; }
        public int ImportOrder { get; set; }
        public Guid? CategoryId { get; set; }
        public bool IsDuplicate { get; set; }
        public virtual Account Account { get; set; }
    }
}