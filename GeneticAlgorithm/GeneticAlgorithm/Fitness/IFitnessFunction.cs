using GeneticAlgorithm.Population;

namespace GeneticAlgorithm.Fitness
{
    public interface IFitnessFunction<T>
    {
        decimal GetValue(T solution);
    }
}