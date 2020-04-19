namespace ReverseMyBudget.MLCategorisation
{
    public class FullPrediction
    {
        public string PredictedLabel { get; }
        public float Score { get; }
        public string Description { get; }
        public string OriginalLabel { get; }

        public FullPrediction(string predictedLabel, float score, string description = null, string originalLabel = null)
        {
            PredictedLabel = predictedLabel;
            Score = score;
            Description = description;
            OriginalLabel = originalLabel;
        }
    }
}