using System;
using GeneticANN.GeneticAlgorithm.Populations;
using GeneticANN.Helpers;

namespace GeneticANN.GeneticAlgorithm.Operators.Mutation
{
    public class GaussMutation : IMutation<DoubleArrayChromosome>
    {
        private readonly double _mutationOperatorThreshold;
        private readonly double _mutationProbability;

        private readonly double _operator1Sigma;
        private readonly double _operator2Sigma;

        private readonly Func<double, double, double> _gaussFunction = (x, sigma) =>
            (1 / (Math.Sqrt(2 * Math.PI * sigma * sigma))) * Math.Exp((-x * x) / (2 * sigma * sigma)); 

        public GaussMutation(double mutationOperatorThreshold, double mutationProbability, double operator1Sigma, 
            double operator2Sigma)
        {
            _mutationOperatorThreshold = mutationOperatorThreshold;
            _mutationProbability = mutationProbability;
            _operator1Sigma = operator1Sigma;
            _operator2Sigma = operator2Sigma;
        }

        public DoubleArrayChromosome Mutate(DoubleArrayChromosome chromosome)
        {
            if (_mutationOperatorThreshold > HelperMethods.Random.NextDouble())
            {
                for (int i = 0; i < chromosome.Values.Length; i++)
                {
                    if (HelperMethods.Random.NextDouble() <= _mutationProbability)
                    {
                        //chromosome[i] += _gaussFunction(chromosome[i], _operator1Sigma);
                        chromosome[i] += (HelperMethods.Random.NextDouble() * 2 - 1) * _operator1Sigma;
                    }
                }
            }
            else
            {
                for (int i = 0; i < chromosome.Values.Length; i++)
                {
                    if (HelperMethods.Random.NextDouble() <= _mutationProbability)
                    {
                        //chromosome[i] = _gaussFunction(chromosome[i], _operator2Sigma);
                        chromosome[i] = (HelperMethods.Random.NextDouble() * 2 - 1) * _operator2Sigma;
                    }
                }
            }

            return chromosome;
        }
    }
}