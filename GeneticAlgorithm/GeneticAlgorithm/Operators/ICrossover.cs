namespace GeneticAlgorithm.Operators
{
    public interface ICrossover<T>
    {
        T Crossover(T chromosome1, T chromosome2);
    }
}