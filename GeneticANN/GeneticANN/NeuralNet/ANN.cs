using System;
using System.Collections.Generic;
using GeneticANN.Dataset;
using GeneticANN.Helpers;

namespace GeneticANN.NeuralNet
{
    public class ANN
    {
        public List<int> NeuronsPerLayer { get; }
        public double[][] Neurons { get; private set; }
        public int NumberOfParameters { get; private set; }

        private int _numberOfLayers;

        public ANN(string architecture)
        {
            NeuronsPerLayer = new List<int>();
            ParseArchitecture(architecture);
            CalculateNumberOfParameters();
        }

        private void ParseArchitecture(string architecture)
        {
            var layers = architecture.Split('x');
            _numberOfLayers = layers.Length;
            Neurons = new double[layers.Length][];

            var index = 0;
            foreach (var layer in layers)
            {
                var neuronsInLayer = int.Parse(layer);
                
                NeuronsPerLayer.Add(neuronsInLayer);
                Neurons[index] = new double[neuronsInLayer];

                index++;
            }
        }

        private void CalculateNumberOfParameters()
        {
            var sum = NeuronsPerLayer[0] * 2 * NeuronsPerLayer[1];

            for (var i = 2; i < NeuronsPerLayer.Count; i++)
            {
                sum += (NeuronsPerLayer[i - 1] + 1) * NeuronsPerLayer[i];
            }

            NumberOfParameters = sum;
        }

        public double[] CalculateOutput(double[] sample, double[] netParameters)
        {
            for (var i = 0; i < NeuronsPerLayer[0]; i++)
            {
                Neurons[0][i] = sample[i];
            }

            var parametersOffset = 0;

            for (var i = 0; i < NeuronsPerLayer[1]; i++)
            {
                Neurons[1][i] = NeuronTypes.NeuronType1(Neurons[0], netParameters, parametersOffset);
                parametersOffset += 2 * NeuronsPerLayer[0];
            }

            for (var i = 2; i < _numberOfLayers; i++)
            {
                for (var j = 0; j < NeuronsPerLayer[i]; j++)
                {
                    Neurons[i][j] = NeuronTypes.NeuronType2(Neurons[i - 1], netParameters, parametersOffset);
                    parametersOffset += NeuronsPerLayer[i - 1] + 1;
                }
            }
            
            return Neurons[_numberOfLayers - 1];
        }

        public double CalculateError(Dataset.Dataset dataset, double[] netParameters)
        {
            var error = 0.0;

            foreach (var sample in dataset)
            {
                var classification = CalculateOutput(sample.Input, netParameters);
                
                for (var i = 0; i < sample.Classification.Length; i++)
                {
                    error += Math.Pow(sample.Classification[i] - classification[i], 2);
                }
            }

            return error / dataset.Count();
        }

        public void WriteNeuronLayerParametersToFile(string filePath, int layer, double[] netParameters)
        {
            var lines = new List<string>();
            
            if (layer == 1)
            {
                var termOffset = 2 * NeuronsPerLayer[0] * NeuronsPerLayer[1];

                for (int i = 0; i < termOffset; i += 4)
                {
                    lines.Add(netParameters[i] + " " + netParameters[i + 1] + " " + netParameters[i + 2] + " " + netParameters[i + 3]);
                }
            }
            
            HelperMethods.WriteToFile(filePath, lines);
        }
    }
}