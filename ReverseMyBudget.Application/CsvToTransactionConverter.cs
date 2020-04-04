using ReverseMyBudget.Domain;
using Serilog;
using System;

namespace ReverseMyBudget.Application
{
    public class CsvToTransactionConverter : ITransactionConverter
    {
        private readonly ILogger _logger;

        public CsvToTransactionConverter(ILogger logger)
        {
            _logger = logger;
        }

        public Transaction Convert(Guid userId, Guid accountId, string line)
        {
            string[] data;

            // Replace ",," with "," "," so that the split always creates the same number of array elements
            line = line.Replace(@""",,""", @""",""Empty"",""");
            line = line.Replace(@""",,,""", @""",""Empty"",""Empty"",""");

            string[] separator = new string[] { "," };
            data = line.Split(separator, StringSplitOptions.None);

            if (data.Length == 7)
            {
                // Ignore first line which contains headings
                if (data[0] == "\"Date")
                {
                    RejectLine(userId, line);

                    return null;
                }

                // Remove remaining quotes and dollar signs
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = data[i].Replace(@"""", "");
                    data[i] = data[i].Replace("$", "");
                }

                // Array order is specific to NAB bank files
                return new Transaction
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
            else
            {
                RejectLine(userId, line);

                return null;
            }
        }

        private void RejectLine(Guid userId, string line)
        {
            _logger.Information(String.Format($"UserId: {userId} {nameof(CsvToTransactionConverter)}: this line was not in the correct format: {0}", line));
        }
    }
}