using System;
using System.Collections.Generic;

namespace Anfis.TrainingSet
{
    public class TrainingSetGenerator
    {
        //((x − 1)^2 + (y + 2)^2 − 5xy + 3) ∗ cos^2 ( x/5 )
        private static readonly Func<double, double, double> Function = (x, y) =>
            (Math.Pow(x - 1, 2) + Math.Pow(y + 2, 2) - 5 * x * y + 3) * Math.Pow(Math.Cos(x / 5), 2);

        public static List<TrainingData> CreateTrainingSet(int xStart, int xEnd, int yStart, int yEnd)
        {
            var trainingSet = new List<TrainingData>();

            for (int i = xStart; i <= xEnd; i++)
            {
                for (int j = yStart; j <= yEnd; j++)
                {
                    var trainingExample = new TrainingData()
                    {
                        X = i,
                        Y = j,
                        F = Function(i, j)
                    };
                    
                    trainingSet.Add(trainingExample);
                }
            }

            return trainingSet;
        }
    }
}