using GeneticANN.NeuralNet;

namespace GeneticANN.GeneticAlgorithm.Fitness
{
    public interface IFitnessFunction<T>
    {
        ANN Ann { get; }
        int GetNumberOfParameters();
        Dataset.Dataset Dataset { get; }
        double CalculateFitness(T chromosome);
    }
}