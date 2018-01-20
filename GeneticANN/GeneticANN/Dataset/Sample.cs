using System;
using System.Globalization;

namespace GeneticANN.Dataset
{
    public class Sample
    {
        public double[] Input { get; }
        public double[] Classification { get; }

        public Sample(string[] input, string[] classification)
        {
            Input = new double[input.Length];

            var index = 0;
            foreach (var s in input)
            {
                Input[index] = double.Parse(s, NumberStyles.Any);
                index++;
            }
            
            Classification = new double[classification.Length];

            index = 0;
            foreach (var s in classification)
            {
                Classification[index] = double.Parse(s, NumberStyles.Any);
                index++;
            }
        }
    }
}