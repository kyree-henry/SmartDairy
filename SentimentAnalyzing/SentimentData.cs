using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalyzing
{
    // Input class
    public class SentimentData
    {
        [LoadColumn(0)]
        public string Text;

        [LoadColumn(1), ColumnName("Label")]
        public bool sentiment;
    }

    // OutPut Class
    public class SentimentPrediction : SentimentData
    {
        [ColumnName("PredictionLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; }
    }
}
