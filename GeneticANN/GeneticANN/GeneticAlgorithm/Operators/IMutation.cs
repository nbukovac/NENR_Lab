namespace GeneticANN.GeneticAlgorithm.Operators
{
    public interface IMutation<T>
    {
        T Mutate(T chromosome);
    }
}