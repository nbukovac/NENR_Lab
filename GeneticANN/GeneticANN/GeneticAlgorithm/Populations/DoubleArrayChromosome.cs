using GeneticANN.Helpers;

namespace GeneticANN.GeneticAlgorithm.Populations
{
    public class DoubleArrayChromosome : IChromosome<double[]>
    {
        public double Fitness { get; set; }
        public double[] Values { get; set; }

        public DoubleArrayChromosome(int numberOfParameters, bool randomize = false)
        {
            Values = new double[numberOfParameters];

            if (randomize)
            {
                for (var i = 0; i < numberOfParameters; i++)
                {
                    Values[i] = HelperMethods.Random.NextDouble() * 2 - 1;
                }
            }
        }

        public double this[int i]
        {
            get => Values[i];
            set => Values[i] = value;
        }

        public int CompareTo(IChromosome<double[]> other)
        {
            if (Fitness > other.Fitness)
            {
                return 1;
            }
            if (Fitness < other.Fitness)
            {
                return -1;
            }

            return 0;
        }
        
    }
}