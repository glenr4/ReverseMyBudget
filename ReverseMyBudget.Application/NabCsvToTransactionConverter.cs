using ReverseMyBudget.Domain;
using ReverseMyBudget.Persistence.Sql;
using Serilog;
using System;

namespace ReverseMyBudget.Application
{
    /// <summary>
    /// National Australia Bank (NAB) CSV to Transaction Converter
    /// </summary>
    public class NabCsvToTransactionConverter : ITransactionConverter
    {
        private readonly ILogger _logger;

        public NabCsvToTransactionConverter(ILogger logger)
        {
            _logger = logger;
        }

        public TransactionStaging Convert(Guid userId, Guid accountId, string line)
        {
            try
            {
                string[] data = this.SplitLine(line);

                this.CheckIfHeadings(data);

                return new TransactionStaging
                {
                    Id = Guid.NewGuid(),
                    DateLocal = System.Convert.ToDateTime(data[0]),
                    Amount = System.Convert.ToDecimal(data[1]),
                    Type = data[4],
                    Description = data[5],
                    Balance = System.Convert.ToDecimal(data[6]),
                    AccountId = accountId,
                    UserId = userId
                };
            }
            catch (Exception ex)
            {
                RejectLine(line, ex.Message);

                return null;
            }
        }

        private string[] SplitLine(string line)
        {
            int expectedLength = 7;
            string[] separator = new string[] { "," };

            string[] data = line.Split(separator, StringSplitOptions.None);

            if (data.Length != expectedLength)
            {
                throw new FormatException($"Expected {expectedLength} items per line but received {data.Length}");
            }

            return data;
        }

        private void CheckIfHeadings(string[] data)
        {
            string headingsFirstItem = "Date";

            // Ignore first line which contains headings
            if (data[0] == headingsFirstItem)
            {
                throw new FormatException("This is a headings line, not a transaction line");
            }
        }

        private void RejectLine(string line, string errorMessage)
        {
            _logger.Information(String.Format($"{nameof(NabCsvToTransactionConverter)}: {errorMessage}: {line}"));
        }
    }
}