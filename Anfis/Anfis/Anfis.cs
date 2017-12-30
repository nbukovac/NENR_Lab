using System;
using System.Collections.Generic;
using System.Linq;
using Anfis.Helpers;
using Anfis.TNorm;
using Anfis.TrainingSet;
using Anfis.TransferFunction;

namespace Anfis
{
    public class Anfis
    {
        
        #region Properties

        public List<TrainingData> TrainingData { get; set; }
        
        public int NumberOfRules { get; set; }
        public ITNorm TNorm { get; set; }
        public ITransferFunction SigmoidTransferFunction { get; set; }
        public bool StochasticGradient { get; set; }
        public double LearningRate { get; set; }
        public int EpochLimit { get; set; }
        public double ErrorTermination { get; set; }

        public double[] A1 { get; set; }
        public double[] A2 { get; set; }
        public double[] B1 { get; set; }
        public double[] B2 { get; set; }
        public double[] P { get; set; }
        public double[] Q { get; set; }
        public double[] R { get; set; }

        #endregion
        
        #region Constructor
        
        public Anfis(int numberOfRules, ITNorm norm, ITransferFunction sigmoidTransferFunction, bool stochasticGradient, 
            double learningRate, int epochLimit, double errorTermination, List<TrainingData> trainingData)
        {
            NumberOfRules = numberOfRules;
            TNorm = norm;
            SigmoidTransferFunction = sigmoidTransferFunction;
            StochasticGradient = stochasticGradient;
            LearningRate = learningRate;
            EpochLimit = epochLimit;
            ErrorTermination = errorTermination;
            TrainingData = trainingData;

            InitArrays();
        }

        private void InitArrays()
        {
            A1 = HelperFunctions.ArrayInitializer(NumberOfRules);
            A2 = HelperFunctions.ArrayInitializer(NumberOfRules);
            B1 = HelperFunctions.ArrayInitializer(NumberOfRules);
            B2 = HelperFunctions.ArrayInitializer(NumberOfRules);
            P = HelperFunctions.ArrayInitializer(NumberOfRules);
            Q = HelperFunctions.ArrayInitializer(NumberOfRules);
            R = HelperFunctions.ArrayInitializer(NumberOfRules);
        }
        
        #endregion
        
        #region Calculation

        private double SigmoidA(double x, int ruleIndex)
        {
            return SigmoidTransferFunction.Calculate(x, A1[ruleIndex], B1[ruleIndex]);
        }

        private double SigmoidB(double y, int ruleIndex)
        {
            return SigmoidTransferFunction.Calculate(y, A2[ruleIndex], B2[ruleIndex]);
        }
        
        private double[] CalculateW(double x, double y)
        {
            var w = new double[NumberOfRules];
            var sum = 0.0;

            for (int i = 0; i < NumberOfRules; i++)
            {
                var a = SigmoidA(x, i);
                var b = SigmoidB(y, i);

                w[i] = TNorm.Calculate(a, b);
                sum += w[i];
            }

            for (int i = 0; i < NumberOfRules; i++)
            {
                w[i] /= sum;
            }

            return w;
        }

        private double[] CalculateZ(double x, double y)
        {
            var z = new double[NumberOfRules];

            for (int i = 0; i < NumberOfRules; i++)
            {
                z[i] = P[i] * x + Q[i] * y + R[i];
            }

            return z;
        }

        private double CalculateF(double x, double y, out double[] w, out double[] z)
        {
            var sum = 0.0;
            w = CalculateW(x, y);
            z = CalculateZ(x, y);

            for (int i = 0; i < NumberOfRules; i++)
            {
                sum += w[i] * z[i];
            }

            return sum;
        }
        
        private double CalculateF(double x, double y)
        {
            var sum = 0.0;
            var w = CalculateW(x, y);
            var z = CalculateZ(x, y);

            for (int i = 0; i < NumberOfRules; i++)
            {
                sum += w[i] * z[i];
            }

            return sum;
        }
        
        #endregion

        public void StartAlgorithm()
        {
            var errorPerEpoch = new List<double>();

            for (int i = 1; i <= EpochLimit; i++)
            {
                if (StochasticGradient)
                {
                    for (int j = 0; j < TrainingData.Count; j++)
                    {
                        CalculateStochasticGradient(j);
                    }
                }
                else
                {
                    
                }

                var error = CalculateError();
                errorPerEpoch.Add(error);

                if (i % 1_000 == 0 || i < 100)
                {
                    Console.WriteLine("Epoch => " + i + "\t\terror => " + error);
                }
                
                if (error < ErrorTermination)
                {
                    break;
                }
            }
        }

        private double CalculateError()
        {
            var errorSum = TrainingData.Sum(sample => Math.Pow(sample.F - CalculateF(sample.X, sample.Y), 2));

            return errorSum / TrainingData.Count;
        }

        #region Gradients

        private void CalculateStochasticGradient(int index)
        {
            var sample = TrainingData[index];
            double[] w;
            double[] z;
            
            var f = CalculateF(sample.X, sample.Y, out w, out z);

            for (int i = 0; i < NumberOfRules; i++)
            {
                var gradientP = GradientP(i, sample, f, w);
                var gradientQ = GradientQ(i, sample, f, w);
                var gradientR = GradientR(i, sample, f, w);

                var gradientA1 = GradientA1(sample, i, f, w, z);
                var gradientA2 = GradientA2(sample, i, f, w, z);
                var gradientB1 = GradientB1(sample, i, f, w, z);
                var gradientB2 = GradientB2(sample, i, f, w, z);
                
                Update(i, gradientP, gradientQ, gradientR, gradientA1, gradientA2, gradientB1, gradientB2);
            }

        }

        private double GradientO(double o, TrainingData sample)
        {
            return o - sample.F;
        }

        private double GradientZ(int index, double[] w)
        {
            return w[index] / w.Sum();
        }
        
        private double GradientP(int index, TrainingData sample, double o, double[] w)
        {
            return GradientO(o, sample) * GradientZ(index, w) * sample.X;
        }

        private double GradientQ(int index, TrainingData sample, double o, double[] w)
        {
            return GradientO(o, sample) * GradientZ(index, w) * sample.Y;
        }

        private double GradientR(int index, TrainingData sample, double o, double[] w)
        {
            return GradientO(o, sample) * GradientZ(index, w);
        }

        private double GradientW(int index, double[] w, double[] z)
        {
            var sumW = w.Sum();
            var sumWZ = 0.0;

            for (int i = 0; i < z.Length; i++)
            {
                sumWZ += w[i] * z[i];
            }

            return (sumW * z[index] - sumWZ) / Math.Pow(sumW, 2);
        }

        private double GradientA1(TrainingData sample, int index, double o, double[] w, double[] z)
        {
            var a = SigmoidA(sample.X, index);
            var gradient = a * B1[index] * (1 - a) * SigmoidB(sample.Y, index);

            return GradientO(o, sample) * GradientW(index, w, z) * gradient;
        }
        
        private double GradientB1(TrainingData sample, int index, double o, double[] w, double[] z)
        {
            var a = SigmoidA(sample.X, index);
            var gradient = -1 * sample.X - a * A1[index] * (1 - a) * SigmoidB(sample.Y, index);

            return GradientO(o, sample) * GradientW(index, w, z) * gradient;
        }
        
        private double GradientA2(TrainingData sample, int index, double o, double[] w, double[] z)
        {
            var b = SigmoidB(sample.Y, index);
            var gradient = b * B2[index] * (1 - b) * SigmoidA(sample.X, index);

            return GradientO(o, sample) * GradientW(index, w, z) * gradient;
        }
        
        private double GradientB2(TrainingData sample, int index, double o, double[] w, double[] z)
        {
            var b = SigmoidB(sample.Y, index);
            var gradient = -1 * sample.Y - b * A2[index] * (1 - b) * SigmoidB(sample.Y, index);

            return GradientO(o, sample) * GradientW(index, w, z) * gradient;
        }

        #endregion

        private void Update(int index, double gradientP, double gradientQ, double gradientR,
            double gradientA1, double gradientA2, double gradientB1, double gradientB2)
        {
            A1[index] -= gradientA1 * LearningRate;
            A2[index] -= gradientA2 * LearningRate;
            B1[index] -= gradientB1 * LearningRate;
            B2[index] -= gradientB2 * LearningRate;

            P[index] -= gradientP * LearningRate * 10;
            Q[index] -= gradientQ * LearningRate * 10;
            R[index] -= gradientR * LearningRate * 10;
        }
    }
}