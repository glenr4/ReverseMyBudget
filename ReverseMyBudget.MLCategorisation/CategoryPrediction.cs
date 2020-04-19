using Microsoft.ML.Data;

namespace ReverseMyBudget.MLCategorisation
{
    internal class CategoryPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Category;

        public float[] Score;

        public string Description { get; set; }
    }
}