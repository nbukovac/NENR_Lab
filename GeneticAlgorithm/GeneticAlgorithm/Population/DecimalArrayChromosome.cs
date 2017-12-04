using System;
using System.Text;
using GeneticAlgorithm.Helpers;

namespace GeneticAlgorithm.Population
{
    public class DecimalArrayChromosome : IChromosome<decimal[]>, IComparable<DecimalArrayChromosome>
    {
        public decimal Fitness { get; set; }
        public decimal[] Values { get; set; }

        public DecimalArrayChromosome(int numberOfElements)
        {
            Values = new decimal[numberOfElements];
        }
        
        
        public void GenerateRandom(decimal[] min, decimal[] max)
        {
            for (var i = 0; i < min.Length; i++)
            {
                Values[i] = (decimal) HelperFunctions.Random.NextDouble() * (max[i] - min[i]) + min[i];
            }
        }

        public decimal this[int i] => Values[i];

        public int CompareTo(IChromosome<decimal[]> other)
        {
            return CompareTo(other as DecimalArrayChromosome);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            
            sb.Append('[');

            for (var i = 0; i < Values.Length; i++)
            {
                sb.Append(Values[i]);
                
                if (i < Values.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append(']');

            return sb.ToString();
        }

        public int CompareTo(DecimalArrayChromosome other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Fitness.CompareTo(other.Fitness);
        }
    }
}