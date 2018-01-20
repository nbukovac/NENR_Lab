using GeneticANN.GeneticAlgorithm.Populations;
using GeneticANN.NeuralNet;

namespace GeneticANN.GeneticAlgorithm.Fitness
{
    public class FitnessFunction : IFitnessFunction<DoubleArrayChromosome>
    {
        public ANN Ann { get; }
        public Dataset.Dataset Dataset { get; }

        public FitnessFunction(ANN ann, Dataset.Dataset dataset)
        {
            Ann = ann;
            Dataset = dataset;
        }

        public int GetNumberOfParameters()
        {
            return Ann.NumberOfParameters;
        }

        public double CalculateFitness(DoubleArrayChromosome chromosome)
        {
            return Ann.CalculateError(Dataset, chromosome.Values);
        }
    }
}