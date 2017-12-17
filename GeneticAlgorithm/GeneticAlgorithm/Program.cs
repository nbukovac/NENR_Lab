using System;
using GeneticAlgorithm.Algorithm;
using GeneticAlgorithm.Fitness;
using GeneticAlgorithm.Operators.Crossover;
using GeneticAlgorithm.Operators.Mutation;
using GeneticAlgorithm.Operators.Selection;
using GeneticAlgorithm.Population;

namespace GeneticAlgorithm
{
    public class Program
    {
        private const int PopulationSize = 30;
        private const decimal FitnessTerminator = (decimal)10e-9;
        private const int IterationLimit = 100_000;
        private const double MutationProbability = 0.1;
        private const int TournamentSize = 3;
        
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("You have to at least specify the data file path");
                return;
            }
            
            var mutation = new UniformMutation(MutationProbability);
            var selection = new KTournamentSelection(TournamentSize);
            var crossover = new ArithmeticCrossover();

            var fitnessFunction = new FitnessFunction(args[0]);

            IGeneticAlgorithm<DecimalArrayChromosome> geneticAlgorithm;

            if (args.Length == 2 && args[1].ToLower() == "gen")
            {
                geneticAlgorithm = new GenerationGeneticAlgorithm(mutation, selection, crossover, fitnessFunction,
                    IterationLimit, FitnessTerminator, PopulationSize);
            }
            else
            {
                geneticAlgorithm = new EliminationGeneticAlgorithm(mutation, selection, crossover, fitnessFunction,
                    IterationLimit, FitnessTerminator, PopulationSize);
            }

            var optimum = geneticAlgorithm.FindOptimum();

            Console.WriteLine();
            Console.WriteLine(optimum);
        }
    }
}