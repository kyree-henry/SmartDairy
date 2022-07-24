using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.IO;
using static Microsoft.ML.DataOperationsCatalog;

namespace SentimentAnalyzing
{
    class Program
    {
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "yelp_labelled.txt");
        static void Main(string[] args)
        {
            MLContext context = new();
            TrainTestData splitDataView = LoadData(context);
            ITransformer model = BuildAndTrainModel(context, splitDataView.TrainSet);
            Evaluate(context, splitDataView.TestSet, model);
            PredictSample(context, model);
            context.Model.Save(model, splitDataView.TrainSet.Schema, "Data/MLModel.zip");
        }


        public static TrainTestData LoadData(MLContext context)
        {
            IDataView dataView = context.Data.LoadFromTextFile<SentimentData>(_dataPath, hasHeader: false);
            TrainTestData splitDataView = context.Data.TrainTestSplit(dataView, testFraction: 0.2);
            return splitDataView;
        }

        public static ITransformer BuildAndTrainModel(MLContext context, IDataView splitTrainSet)
        {
            var pipeLine = context.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(SentimentData.Text))
                .Append(context.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

            var model = pipeLine.Fit(splitTrainSet);
            return model;
        }

        public static void Evaluate(MLContext context, IDataView splitTestSet, ITransformer model)
        {
            IDataView prediction = model.Transform(splitTestSet);
            CalibratedBinaryClassificationMetrics metrics = context.BinaryClassification.Evaluate(prediction, "Label");
            Console.WriteLine("========================");
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine("========================");
        }

        private static void PredictSample(MLContext mlContext, ITransformer model)
        {
            PredictionEngine<SentimentData, SentimentPrediction> predictionFunction =
                mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
            SentimentData sampleStatement = new SentimentData
            {
                Text = "Today been make sense"
            };
            var resultPrediction = predictionFunction.Predict(sampleStatement);
            Console.WriteLine();
            Console.WriteLine("=============== Prediction Test of model with a single sample and test dataset ===============");

            Console.WriteLine();
            Console.WriteLine($"Sentiment: {resultPrediction.Text} | Prediction: " +
                $"{(Convert.ToBoolean(resultPrediction.Prediction) ? "Positive" : "Negative")} | Probability: {resultPrediction.Probability} ");

            Console.WriteLine("=============== End of Predictions ===============");
            Console.WriteLine();
        }

       
    }

    public class Sern
    {
        public SentimentPrediction PredictSentiment(MLContext context, string text)
        {
            TrainTestData splitDataView = Program.LoadData(context);
            ITransformer model = Program.BuildAndTrainModel(context, splitDataView.TrainSet);

            PredictionEngine<SentimentData, SentimentPrediction> predictionFunction =
                context.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
            SentimentData sampleStatement = new()
            {
                Text = text
            };
            return predictionFunction.Predict(sampleStatement);
        }
    }

    
}
