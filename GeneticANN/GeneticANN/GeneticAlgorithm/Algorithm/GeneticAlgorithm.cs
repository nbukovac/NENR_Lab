using System.Collections.Generic;
using GeneticANN.GeneticAlgorithm.Fitness;
using GeneticANN.GeneticAlgorithm.Operators;
using GeneticANN.GeneticAlgorithm.Populations;

namespace GeneticANN.GeneticAlgorithm.Algorithm
{
    public abstract class GeneticAlgorithm<T> : IGeneticAlgorithm<T>
    {
        public IMutation<T> Mutation { get; }
        public ISelection<T> Selection { get; }
        public List<ICrossover<T>> Crossovers { get; }
        public int IterationLimit { get; }
        public IFitnessFunction<T> FitnessFunction { get; }
        public double FitnessTerminator { get; }
        public int PopulationSize { get; }
        protected Population<T> Population { get; set; }

        protected GeneticAlgorithm(IMutation<T> mutation, ISelection<T> selection, List<ICrossover<T>> crossovers, 
           IFitnessFunction<T> fitnessFunction, int iterationLimit, double fitnessTerminator, int populationSize)
        {
            Mutation = mutation;
            Selection = selection;
            Crossovers = crossovers;
            FitnessFunction = fitnessFunction;
            IterationLimit = iterationLimit;
            FitnessTerminator = fitnessTerminator;
            PopulationSize = populationSize;
        }

        public abstract T FindOptimum();
    }
}