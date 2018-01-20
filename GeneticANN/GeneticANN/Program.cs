using System;
using System.Collections.Generic;
using System.Linq;
using GeneticANN.GeneticAlgorithm.Algorithm;
using GeneticANN.GeneticAlgorithm.Fitness;
using GeneticANN.GeneticAlgorithm.Operators;
using GeneticANN.GeneticAlgorithm.Operators.Crossover;
using GeneticANN.GeneticAlgorithm.Operators.Mutation;
using GeneticANN.GeneticAlgorithm.Operators.Selection;
using GeneticANN.GeneticAlgorithm.Populations;
using GeneticANN.NeuralNet;

namespace GeneticANN
{
    public static class Program
    {
        private const int IterationLimit = 3_000_000;
        private const int TournamentSize = 3;
        private const int PopulationSize = 30;
        private const double MutationProbability = 0.05;
        private const double MutationThreshold = 0.6;
        private const double Sigma1 = 1;
        private const double Sigma2 = 1;
        private const double ErrorLimit = 10e-7;
        private const string Architecture = "2x8x4x3";
        
        private const string DatasetFilePath = "dataset.txt";
        private const string ParametersFilePath = "parameters2.txt";
        
        public static void Main(string[] args)
        {
            var dataset = new Dataset.Dataset(DatasetFilePath);
            var ann = new ANN(Architecture);
            
            var fitness = new FitnessFunction(ann, dataset);

            var mutation = new GaussMutation(MutationThreshold, MutationProbability, Sigma1, Sigma2);
            var selection = new KTournamentSelection<DoubleArrayChromosome>(TournamentSize);
            var crossovers = new List<ICrossover<DoubleArrayChromosome>>()
            {
                new ArithmeticCrossover(),
                new HeuristicCrossover(),
                new UniformCrossover()
            };

            var geneticAlgorithm =
                new EliminationGeneticAlgorithm(mutation, selection, crossovers, fitness, IterationLimit, ErrorLimit, PopulationSize);

            var optimum = geneticAlgorithm.FindOptimum();
            var correctClassification = 0;

            foreach (var sample in dataset)
            {
                var classification = ann.CalculateOutput(sample.Input, optimum.Values);
                var correct = true;

                for (int i = 0; i < classification.Length; i++)
                {
                    classification[i] = classification[i] < 0.5 ? 0 : 1;
                    if (Math.Abs(classification[i] - sample.Classification[i]) > 10e-9)
                    {
                        correct = false;
                    }
                }

                Console.WriteLine(classification[0] + " " + classification[1] + " " + classification[2] + 
                                  " <=> " + sample.Classification[0] + " " + sample.Classification[1] + " "+ sample.Classification[2] + " ");
                
                if (correct)
                {
                    correctClassification++;
                }
                
            }
            
            Console.WriteLine("Correct => " + correctClassification + ", Total => " + dataset.Count());
            
            ann.WriteNeuronLayerParametersToFile(ParametersFilePath, 1, optimum.Values);
        }
    }
}