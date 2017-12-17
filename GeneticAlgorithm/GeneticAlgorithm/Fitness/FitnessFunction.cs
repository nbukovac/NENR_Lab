using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using GeneticAlgorithm.Population;

namespace GeneticAlgorithm.Fitness
{
    public class FitnessFunction : IFitnessFunction<DecimalArrayChromosome>
    {
        private readonly List<decimal> _x;
        private readonly List<decimal> _y;
        private readonly List<decimal> _f;

        private static readonly Func<DecimalArrayChromosome, decimal, decimal, decimal> FitnessFunc = (v, x, y) =>
            (decimal) (Math.Sin((double) (v[0] + v[1] * x)) + (double) v[2] * Math.Cos((double) (x * (v[3] + y))) *
                       (1.0 / (1 + Math.Exp(Math.Pow((double) (x - v[4]), 2)))));

        public FitnessFunction(string filePath)
        {
            _x = new List<decimal>();
            _y = new List<decimal>();
            _f = new List<decimal>();
            
            ExtractDataFromFile(filePath);
        }

        private void ExtractDataFromFile(string filePath)
        {
            using (var inputStream = new StreamReader(filePath))
            {
                var line = string.Empty;

                while ((line = inputStream.ReadLine()) != null)
                {
                    var splitData = line.Trim().Split('\t');

                    if (splitData.Length != 3)
                    {
                        continue;
                    }
                    
                    _x.Add(decimal.Parse(splitData[0], NumberStyles.Any));
                    _y.Add(decimal.Parse(splitData[1], NumberStyles.Any));
                    _f.Add(decimal.Parse(splitData[2], NumberStyles.Any));
                }
            }
        }
        
        public decimal GetValue(DecimalArrayChromosome chromosome)
        {
            var sum = 0.0M;

            for (var i = 0; i < _x.Count; i++)
            {
                sum += (decimal) Math.Pow((double)(FitnessFunc(chromosome, _x[i], _y[i]) - _f[i]), 2);
            }

            return sum / _x.Count;
        }
    }
}