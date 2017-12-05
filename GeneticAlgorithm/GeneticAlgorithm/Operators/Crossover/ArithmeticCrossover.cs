using GeneticAlgorithm.Helpers;
using GeneticAlgorithm.Population;

namespace GeneticAlgorithm.Operators.Crossover
{
    public class ArithmeticCrossover : ICrossover<DecimalArrayChromosome>
    {
        
        public DecimalArrayChromosome Cross(DecimalArrayChromosome chromosome1, DecimalArrayChromosome chromosome2)
        {
            var length = chromosome1.Values.Length;
            var newValues = new decimal[length];
            
            for (int i = 0; i < length; i++)
            {
                var rand = (decimal) HelperFunctions.Random.NextDouble();
                newValues[i] = rand * chromosome1.Values[i] + (1 - rand) * chromosome2.Values[i];
            }
            
            return new DecimalArrayChromosome(length)
            {
                Values = newValues
            };
        }
    }
}