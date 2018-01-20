using GeneticANN.GeneticAlgorithm.Populations;
using GeneticANN.Helpers;

namespace GeneticANN.GeneticAlgorithm.Operators.Crossover
{
    public class UniformCrossover : ICrossover<DoubleArrayChromosome>
    {
        public DoubleArrayChromosome Cross(DoubleArrayChromosome chromosome1, DoubleArrayChromosome chromosome2)
        {
            var child = new DoubleArrayChromosome(chromosome1.Values.Length);

            for (int i = 0; i < chromosome1.Values.Length; i++)
            {
                if (HelperMethods.Random.NextDouble() < 0.5)
                {
                    child[i] = chromosome1[i];
                }
                else
                {
                    child[i] = chromosome2[i];
                }
            }

            return child;
        }
    }
}