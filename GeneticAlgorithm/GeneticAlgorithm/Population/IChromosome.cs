using System;

namespace GeneticAlgorithm.Population
{
    public interface IChromosome<T> : IComparable<IChromosome<T>>
    {
        decimal Fitness { get; set; }
        void GenerateRandom(T min, T max);
    }
}