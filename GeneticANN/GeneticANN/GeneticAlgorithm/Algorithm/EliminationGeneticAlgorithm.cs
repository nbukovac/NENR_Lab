using System;
using System.Linq;
using GeneticANN.GeneticAlgorithm.Fitness;
using GeneticANN.GeneticAlgorithm.Operators;
using GeneticANN.GeneticAlgorithm.Populations;

namespace GeneticANN.GeneticAlgorithm.Algorithm
{
    public class EliminationGeneticAlgorithm : GeneticAlgorithm<DoubleArrayChromosome>
    {
        public EliminationGeneticAlgorithm(IMutation<DoubleArrayChromosome> mutation, ISelection<DoubleArrayChromosome> selection, 
            ICrossover<DoubleArrayChromosome> crossover, IFitnessFunction<DoubleArrayChromosome> fitnessFunction, int iterationLimit, 
            double fitnessTerminator, int populationSize) 
            : base(mutation, selection, crossover, fitnessFunction, iterationLimit, fitnessTerminator, populationSize)
        {
            Population = new Population<DoubleArrayChromosome>(populationSize);
            InitializePopulation();
        }

        private void InitializePopulation()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                var chromosome = new DoubleArrayChromosome(FitnessFunction.GetNumberOfParameters(), randomize: true);
                chromosome.Fitness = FitnessFunction.CalculateFitness(chromosome);
                Population.AddChromosome(chromosome);
            }
        }

        public override DoubleArrayChromosome FindOptimum()
        {
            var best = new DoubleArrayChromosome(FitnessFunction.GetNumberOfParameters()) { Fitness = double.MaxValue };

            for (var i = 0; i < IterationLimit; i++)
            {
                var selectedFromPopulation = Selection.Select(Population).OrderBy(x => x.Fitness).ToList();
                
                var childChromosome = Crossover.Cross(selectedFromPopulation[0], selectedFromPopulation[1]);
                childChromosome = Mutation.Mutate(childChromosome);
                childChromosome.Fitness = FitnessFunction.CalculateFitness(childChromosome);
                
                Population.RemoveChromosome(selectedFromPopulation[2]);
                Population.AddChromosome(childChromosome);

                var populationBest = double.MaxValue;
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
                    best = Population[bestIndex];
                    Console.WriteLine("Iteration: " + i + ", Best fitness: " + populationBest);
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