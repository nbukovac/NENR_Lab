using System.Collections.Generic;
using GeneticAlgorithm.Population;

namespace GeneticAlgorithm.Operators
{
    public interface ISelection<T> where T : DecimalArrayChromosome
    {
        List<T> Select(Population<T> population);
    }
}