using System.Collections.Generic;
using GeneticAlgorithm.Population;

namespace GeneticAlgorithm.Operators
{
    public interface ISelection<T>
    {
        List<T> Select(Population<T> population);
    }
}