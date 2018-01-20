using System;
using System.Collections.Generic;
using System.IO;
using Anfis.Helpers;
using Anfis.TNorm;
using Anfis.TrainingSet;
using Anfis.TransferFunction;

namespace Anfis
{
    public static class Program
    {
        private const int NumberOfRules = 8;
        private const double LearningRate = 0.0001;
        private const bool Stohastic = true;
        
        public static void Main(string[] args)
        {
            var dataSet = CreateTrainingSet();
            var anfis = new Anfis(NumberOfRules, new ProductTNorm(), new SigmoidalTransferFunction(), Stohastic, 
                LearningRate, 100_000, 10e-5, dataSet);
            
            anfis.StartAlgorithm();
            
            //anfis.WriteOutputError("diff_" + NumberOfRules);
            //var rules = anfis.WriteFuzzySetRules("rules_" + NumberOfRules);
            //TransformRuleFile(rules, Environment.CurrentDirectory + "/plot_" + NumberOfRules);
            //anfis.WriteErrors("errors_" + NumberOfRules + (Stohastic ? "_stohastic" : "_gradient"));
        }

        private static List<TrainingData> CreateTrainingSet()
        {
            var trainingSet = TrainingSetGenerator.CreateTrainingSet(Constants.XLowerBound, Constants.XUpperBound, 
                Constants.YLowerBound, Constants.YUpperBound);

            return trainingSet;
        }

        private static void TransformRuleFile(IEnumerable<string> rules, string filename)
        {
            using (var writer = new StreamWriter(filename))
            {
                foreach (var line in rules)
                {
                    var split = line.Split('\t', StringSplitOptions.RemoveEmptyEntries);

                    var a1 = split[0].Split(' ')[2];
                    var b1 = split[1].Split(' ')[2];
                    
                    writer.WriteLine("plot 1 / (1 + exp(" + a1 + " * (x - " + b1 + ")))");

                    var a2 = split[2].Split(' ')[2];
                    var b2 = split[3].Split(' ')[2];
                    
                    writer.WriteLine("plot 1 / (1 + exp(" + a2 + " * (x - " + b2+ ")))");
                }
            }
        }
    }
}