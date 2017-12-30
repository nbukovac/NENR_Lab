using System;
using System.Collections.Generic;
using Anfis.Helpers;
using Anfis.TNorm;
using Anfis.TrainingSet;
using Anfis.TransferFunction;

namespace Anfis
{
    class Program
    {
        
        public static void Main(string[] args)
        {
            var dataSet = CreateTrainingSet();
            var anfis = new Anfis(3, new ProductTNorm(), new SigmoidalTransferFunction(), true, 0.00001, 100_000, 10e-5, dataSet);
            anfis.StartAlgorithm();
        }

        private static List<TrainingData> CreateTrainingSet()
        {
            var trainingSet = TrainingSetGenerator.CreateTrainingSet(Constants.XLowerBound, Constants.XUpperBound, 
                Constants.YLowerBound, Constants.YUpperBound);

            return trainingSet;
        }
    }
}