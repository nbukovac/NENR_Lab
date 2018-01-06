using System;
using System.Collections.Generic;
using Anfis.Helpers;
using Anfis.TNorm;
using Anfis.TrainingSet;
using Anfis.TransferFunction;

namespace Anfis
{
    public static class Program
    {
        
        public static void Main(string[] args)
        {
            var dataSet = CreateTrainingSet();
            var anfis = new Anfis(5, new ProductTNorm(), new SigmoidalTransferFunction(), false, 0.001, 100_000, 10e-5, dataSet);
            anfis.StartAlgorithm();
            
            anfis.WriteFuzzySetRules("fuzzy_rules");
        }

        private static List<TrainingData> CreateTrainingSet()
        {
            var trainingSet = TrainingSetGenerator.CreateTrainingSet(Constants.XLowerBound, Constants.XUpperBound, 
                Constants.YLowerBound, Constants.YUpperBound);

            return trainingSet;
        }
    }
}