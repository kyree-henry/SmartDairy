using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ML.DataOperationsCatalog;

namespace SentimentAnalyzing.Services
{
    public static class Analysis
    {
        static readonly string _dataPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\yelp_labelled.txt");

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

        public static SentimentPrediction GetSentiment(this MLContext context, string text)
        {
            TrainTestData splitDataView = LoadData(context);
            ITransformer model = BuildAndTrainModel(context, splitDataView.TrainSet);

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
