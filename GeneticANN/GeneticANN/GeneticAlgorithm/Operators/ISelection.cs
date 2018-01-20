using System.Collections.Generic;
using GeneticANN.GeneticAlgorithm.Populations;

namespace GeneticANN.GeneticAlgorithm.Operators
{
    public interface ISelection<T>
    {
        List<T> Select(Population<T> population);
    }
}