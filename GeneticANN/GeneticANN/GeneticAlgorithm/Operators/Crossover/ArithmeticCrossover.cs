using GeneticANN.GeneticAlgorithm.Populations;
using GeneticANN.Helpers;

namespace GeneticANN.GeneticAlgorithm.Operators.Crossover
{
    public class ArithmeticCrossover : ICrossover<DoubleArrayChromosome>
    {
        public DoubleArrayChromosome Cross(DoubleArrayChromosome chromosome1, DoubleArrayChromosome chromosome2)
        {
            var child = new DoubleArrayChromosome(chromosome1.Values.Length);

            for (int i = 0; i < chromosome1.Values.Length; i++)
            {
                var rand = HelperMethods.Random.NextDouble();
                child[i] = rand * chromosome1[i] + (1 - rand) * chromosome2[i];
            }

            return child;
        }
    }
}