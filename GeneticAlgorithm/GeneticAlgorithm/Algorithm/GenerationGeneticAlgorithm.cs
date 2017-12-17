using System;
using System.Linq;
using GeneticAlgorithm.Fitness;
using GeneticAlgorithm.Helpers;
using GeneticAlgorithm.Operators;
using GeneticAlgorithm.Population;

namespace GeneticAlgorithm.Algorithm
{
    public class GenerationGeneticAlgorithm : GeneticAlgorithm<DecimalArrayChromosome>
    {
        public bool Elitism { get; }
        
        public GenerationGeneticAlgorithm(IMutation<DecimalArrayChromosome> mutation, ISelection<DecimalArrayChromosome> selection, 
            ICrossover<DecimalArrayChromosome> crossover, IFitnessFunction<DecimalArrayChromosome> fitnessFunction, int iterationLimit, 
            decimal fitnessTerminator, int populationSize, bool elitism = true) : base(mutation, selection, crossover, fitnessFunction, iterationLimit, fitnessTerminator, populationSize)
        {
            Elitism = elitism;
            
            Population = new Population<DecimalArrayChromosome>(populationSize);
            InitializePopulation();
        }
        
        private void InitializePopulation()
        {
            var minArray = HelperFunctions.GetArray(Constants.LowerBoundary, Constants.ChromosomeSize);
            var maxArray = HelperFunctions.GetArray(Constants.UpperBoundary, Constants.ChromosomeSize);

            for (var i = 0; i < PopulationSize; i++)
            {
                var chromosome = new DecimalArrayChromosome(Constants.ChromosomeSize);
                chromosome.GenerateRandom(minArray, maxArray);
                chromosome.Fitness = FitnessFunction.GetValue(chromosome);
                Population.Add(chromosome);
            }
        }

        public override DecimalArrayChromosome FindOptimum()
        {
            var best = new DecimalArrayChromosome(Constants.ChromosomeSize) { Fitness = decimal.MaxValue };

            for (var i = 0; i < IterationLimit; i++)
            {
                var nextGeneration = new Population<DecimalArrayChromosome>(PopulationSize);

                if (Elitism)
                {
                    nextGeneration.Add(GetPopulationBest());
                }

                while (nextGeneration.Count() < PopulationSize)
                {
                    var selectedFromPopulation = Selection.Select(Population);
                    var childChromosome =
                        Crossover.Cross(selectedFromPopulation.First(), selectedFromPopulation[1]);
                    childChromosome = Mutation.Mutate(childChromosome);
                    childChromosome.Fitness = FitnessFunction.GetValue(childChromosome);
                    
                    nextGeneration.Add(childChromosome);
                }

                Population = nextGeneration;
                var populationBest = GetPopulationBest();
                
                if (populationBest.Fitness < best.Fitness)
                {
                    best = populationBest;
                    Console.WriteLine("Iteration: " + i + ", Best fitness: " + populationBest.Fitness + ", " + best);
                }

                if (populationBest.Fitness < FitnessTerminator)
                {
                    break;
                }
            }

            return best;
        }

        private DecimalArrayChromosome GetPopulationBest()
        {
            var populationBest = decimal.MaxValue;
            var bestIndex = 0;
            var index = 0;

            foreach (var chromosome in Population)
            {
                if (chromosome.Fitness < populationBest)
                {
                    populationBest = chromosome.Fitness;
                    bestIndex = index;
                }

                index++;
            }

            return Population.GetAtIndex(bestIndex);
        }
    }
}