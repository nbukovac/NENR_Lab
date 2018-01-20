using System;

namespace GeneticANN.NeuralNet
{
    public static class NeuronTypes
    {
        public static double NeuronType1(double[] sample, double[] netParameters, int parametersOffset)
        {
            var sum = 0.0;

            for (var i = 0; i < sample.Length; i++)
            {
                // index position => parameter wi; index + 1 position => parameter si
                var index = 2 * i + parametersOffset;
                sum += Math.Abs(sample[i] - netParameters[index]) / Math.Abs(netParameters[index + 1]);
            }
            
            return 1 / (1 + sum);
        }

        public static double NeuronType2(double[] sample, double[] netParameters, int parametersOffset)
        {
            var sum = 0.0;
            var index = 0;

            for (var i = 0; i < sample.Length; i++)
            {
                // index position => weight parameter
                index = parametersOffset + i;
                sum += netParameters[index] * sample[i];
            }
            
            var bias = netParameters[index + 1];
            
            return 1 / (1 + Math.Exp(-sum - bias));
        }
    }
}