using GeneticANN.GeneticAlgorithm.Populations;
using GeneticANN.Helpers;

namespace GeneticANN.GeneticAlgorithm.Operators.Mutation
{
    public class UniformMutation : IMutation<DoubleArrayChromosome>
    {
        private readonly double _mutationProbability;

        public UniformMutation(double mutationProbability)
        {
            _mutationProbability = mutationProbability;
        }
        
        public DoubleArrayChromosome Mutate(DoubleArrayChromosome chromosome)
        {
            for (int i = 0; i < chromosome.Values.Length; i++)
            {
                if (HelperMethods.Random.NextDouble() < _mutationProbability)
                {
                    chromosome[i] = HelperMethods.Random.NextDouble() * 2 - 1;
                }
            }

            return chromosome;
        }
    }
}