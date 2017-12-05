using System;
using GeneticAlgorithm.Algorithm;
using GeneticAlgorithm.Fitness;
using GeneticAlgorithm.Operators.Crossover;
using GeneticAlgorithm.Operators.Mutation;
using GeneticAlgorithm.Operators.Selection;

namespace GeneticAlgorithm
{
    public class Program
    {
        private const int PopulationSize = 30;
        private const decimal FitnessTerminator = (decimal)10e-12;
        private const int IterationLimit = 100_000;
        private const double MutationProbability = 0.1;
        private const int TournamentSize = 3;
        
        public static void Main(string[] args)
        {
            var mutation = new UniformMutation(MutationProbability);
            var selection = new KTournamentSelection(TournamentSize);
            var crossover = new ArithmeticCrossover();

            var fitnessFunction = new FitnessFunction(args[0]);

            var geneticAlgorithm = new GenerationGeneticAlgorithm(mutation, selection, crossover, fitnessFunction,
                IterationLimit, FitnessTerminator, PopulationSize);

            var optimum = geneticAlgorithm.FindOptimum();

            Console.WriteLine();
            Console.WriteLine(optimum);
        }
    }
}