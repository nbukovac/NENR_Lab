using System;

namespace GeneticANN.GeneticAlgorithm.Populations
{
    public interface IChromosome<T> : IComparable<IChromosome<T>>
    {
        double Fitness { get; set; }
        T Values { get; set; }
    }
}