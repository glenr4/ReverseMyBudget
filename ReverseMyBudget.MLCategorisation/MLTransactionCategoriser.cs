using Mapster;
using Microsoft.ML;
using Microsoft.ML.Data;
using ReverseMyBudget.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReverseMyBudget.MLCategorisation
{
    public class MLTransactionCategoriser : ITransactionCategoriser
    {
        private readonly PredictionEngine<CategorisedTransaction, CategoryPrediction> _predictionEngine;

        private string _modelPath = @"..\ReverseMyBudget.MLCategorisation\model\model.zip";

        public MLTransactionCategoriser()
        {
            var mlContext = new MLContext();

            var trainedModel = mlContext.Model.Load(_modelPath, out var modelInputSchema);

            _predictionEngine = mlContext.Model.CreatePredictionEngine<CategorisedTransaction, CategoryPrediction>(trainedModel);
        }

        public IEnumerable<FullPrediction> CategoriseTransactions(IEnumerable<Transaction> transactions)
        {
            var categorisedTransactions = transactions.Adapt<IEnumerable<CategorisedTransaction>>();

            return categorisedTransactions.Select(ct => this.GetBestPrediction(ct));
        }

        private FullPrediction GetBestPrediction(CategorisedTransaction transaction)
        {
            CategoryPrediction prediction = _predictionEngine.Predict(transaction);

            VBuffer<ReadOnlyMemory<char>> categories = default;
            _predictionEngine.OutputSchema[nameof(CategoryPrediction.Score)].GetSlotNames(ref categories);

            var scoresList = prediction.Score.ToList();
            var orderedScores = scoresList.OrderByDescending(s => s).ToList();

            return new FullPrediction(
                categories.GetItemOrDefault(scoresList.IndexOf(orderedScores[0])).ToString(),
                orderedScores[0],
                transaction.Description,
                transaction.Category);
        }
    }
}