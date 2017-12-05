using System;
using GeneticAlgorithm.Fitness;
using GeneticAlgorithm.Helpers;
using GeneticAlgorithm.Operators;
using GeneticAlgorithm.Population;

namespace GeneticAlgorithm.Algorithm
{
    public class EliminationGeneticAlgorithm : GeneticAlgorithm<DecimalArrayChromosome>
    {
        public EliminationGeneticAlgorithm(IMutation<DecimalArrayChromosome> mutation, ISelection<DecimalArrayChromosome> selection, 
            ICrossover<DecimalArrayChromosome> crossover, IFitnessFunction<DecimalArrayChromosome> fitnessFunction, int iterationLimit, decimal fitnessTerminator, 
            int populationSize) : base(mutation, selection, crossover, fitnessFunction, iterationLimit, fitnessTerminator, populationSize)
        {
            Population = new Population<DecimalArrayChromosome>(populationSize);
            InitializePopulation();
        }

        private void InitializePopulation()
        {
            var minArray = HelperFunctions.GetArray(Constants.LowerBoundary, Constants.ChromosomeSize);
            var maxArray = HelperFunctions.GetArray(Constants.UpperBoundary, Constants.ChromosomeSize);

            for (int i = 0; i < PopulationSize; i++)
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
                var selectedFromPopulation = Selection.Select(Population);
                
                var childChromosome = Crossover.Cross(selectedFromPopulation[0], selectedFromPopulation[1]);
                childChromosome = Mutation.Mutate(childChromosome);
                childChromosome.Fitness = FitnessFunction.GetValue(childChromosome);
                
                Population.Remove(selectedFromPopulation[2]);
                Population.Add(childChromosome);

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

                if (populationBest < best.Fitness)
                {
                    best = Population.GetAtIndex(bestIndex);
                    Console.WriteLine("Iteration: " + i + ", Best fitness: " + populationBest + ", " + best);
                }

                if (populationBest < FitnessTerminator)
                {
                    break;
                }
            }

            return best;
        }
    }
}