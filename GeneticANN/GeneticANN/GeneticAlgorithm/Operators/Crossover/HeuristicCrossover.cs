using GeneticANN.GeneticAlgorithm.Populations;
using GeneticANN.Helpers;

namespace GeneticANN.GeneticAlgorithm.Operators.Crossover
{
    public class HeuristicCrossover : ICrossover<DoubleArrayChromosome>
    {
        public DoubleArrayChromosome Cross(DoubleArrayChromosome chromosome1, DoubleArrayChromosome chromosome2)
        {
            var child = new DoubleArrayChromosome(chromosome1.Values.Length);
            var betterParent = chromosome1.Fitness > chromosome2.Fitness ? chromosome1 : chromosome2;
            var worseParent = chromosome1 == betterParent ? chromosome2 : chromosome1;

            var rand = HelperMethods.Random.NextDouble();

            for (int i = 0; i < chromosome1.Values.Length; i++)
            {
                child[i] = rand * (betterParent[i] - worseParent[i]) + betterParent[i];
            }

            return child;
        }
    }
}