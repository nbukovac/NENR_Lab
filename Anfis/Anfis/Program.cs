using System;
using Anfis.TrainingSet;

namespace Anfis
{
    class Program
    {
        private const int XLowerBound = -4;
        private const int XUpperBound = 4;
        private const int YLowerBound = -4;
        private const int YUpperBound = 4;
        
        public static void Main(string[] args)
        {
            CreateTrainingSet();
        }

        private static void CreateTrainingSet()
        {
            var trainingSet = TrainingSetGenerator.CreateTrainingSet(XLowerBound, XUpperBound, YLowerBound, YUpperBound);
            
            Console.WriteLine(trainingSet.Count);
            
            trainingSet.ForEach(Console.WriteLine);
        }
    }
}