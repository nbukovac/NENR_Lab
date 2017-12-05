using GeneticAlgorithm.Helpers;
using GeneticAlgorithm.Population;

namespace GeneticAlgorithm.Operators.Mutation
{
    public class UniformMutation : Mutation<DecimalArrayChromosome>
    {
        public UniformMutation(double mutationProbability) : base(mutationProbability)
        {
        }
        
        public override DecimalArrayChromosome Mutate(DecimalArrayChromosome chromosome)
        {
            for (var i = 0; i < chromosome.Values.Length; i++)
            {
                if (HelperFunctions.Random.NextDouble() <= _mutationProbability)
                {
                    chromosome.Values[i] = HelperFunctions.GenerateRandomValue(Constants.LowerBoundary, Constants.UpperBoundary);
                }
            }

            return chromosome;
        }
    }
}