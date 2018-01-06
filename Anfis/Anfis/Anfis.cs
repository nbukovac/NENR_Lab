using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anfis.Helpers;
using Anfis.TNorm;
using Anfis.TrainingSet;
using Anfis.TransferFunction;

namespace Anfis
{
    public class Anfis
    {
        
        #region Properties

        private List<TrainingData> TrainingData { get; }

        private int NumberOfRules { get; }
        private ITNorm TNorm { get; }
        private ITransferFunction SigmoidTransferFunction { get; }
        private bool StochasticGradient { get; }
        private double LearningRate { get; }
        private int EpochLimit { get; }
        private double ErrorTermination { get; }

        private double[] A1 { get; set; }
        private double[] A2 { get; set; }
        private double[] B1 { get; set; }
        private double[] B2 { get; set; }
        private double[] P { get; set; }
        private double[] Q { get; set; }
        private double[] R { get; set; }

        private List<string> ErrorsPerEpoch { get; }
        
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
            
            ErrorsPerEpoch = new List<string>();
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
            for (int epoch = 1; epoch <= EpochLimit; epoch++)
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
                    CalculateGradient();
                }

                var error = CalculateError();
                ErrorsPerEpoch.Add(epoch + ",\t" + error);

                if (epoch % 1_000 == 0)
                {
                    Console.WriteLine("Epoch => " + epoch + "\t\terror => " + error);
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

        #region Gradient algorithms

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

        private void CalculateGradient()
        {
            var gradientP = HelperFunctions.ArrayInitializer(NumberOfRules, false);
            var gradientQ = HelperFunctions.ArrayInitializer(NumberOfRules, false);
            var gradientR = HelperFunctions.ArrayInitializer(NumberOfRules, false);
            var gradientA1 = HelperFunctions.ArrayInitializer(NumberOfRules, false);
            var gradientA2 = HelperFunctions.ArrayInitializer(NumberOfRules, false);
            var gradientB1 = HelperFunctions.ArrayInitializer(NumberOfRules, false);
            var gradientB2 = HelperFunctions.ArrayInitializer(NumberOfRules, false);

            foreach (var sample in TrainingData)
            {
                var f = CalculateF(sample.X, sample.Y, out var w, out var z);

                
                for (int i = 0; i < NumberOfRules; i++)
                {
                    gradientP[i] += GradientP(i, sample, f, w);
                    gradientQ[i] += GradientQ(i, sample, f, w);
                    gradientR[i] += GradientR(i, sample, f, w);

                    gradientA1[i] += GradientA1(sample, i, f, w, z);
                    gradientA2[i] += GradientA2(sample, i, f, w, z);
                    gradientB1[i] += GradientB1(sample, i, f, w, z);
                    gradientB2[i] += GradientB2(sample, i, f, w, z);
                }
            }

            for (int i = 0; i < NumberOfRules; i++)
            {
                Update(i, gradientP[i], gradientQ[i], gradientR[i], gradientA1[i], gradientA2[i], gradientB1[i], gradientB2[i]);
            }
        }
        
        #endregion

        #region Gradients
        
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
                sumWZ += w[i] * (z[index] - z[i]);
            }

            return sumWZ / Math.Pow(sumW, 2);
        }

        private double GradientA1(TrainingData sample, int index, double o, double[] w, double[] z)
        {
            var miA = SigmoidA(sample.X, index);
            var gradient = B1[index] * miA * (1 - miA) * SigmoidB(sample.Y, index);

            return GradientO(o, sample) * GradientW(index, w, z) * gradient;
        }
        
        private double GradientB1(TrainingData sample, int index, double o, double[] w, double[] z)
        {
            var miA = SigmoidA(sample.X, index);
            var gradient = -1 * (sample.X - A1[index]) * miA * (1 - miA) * SigmoidB(sample.Y, index);

            return GradientO(o, sample) * GradientW(index, w, z) * gradient;
        }
        
        private double GradientA2(TrainingData sample, int index, double o, double[] w, double[] z)
        {
            var miB = SigmoidB(sample.Y, index);
            var gradient = B2[index] * miB * (1 - miB) * SigmoidA(sample.X, index);

            return GradientO(o, sample) * GradientW(index, w, z) * gradient;
        }
        
        private double GradientB2(TrainingData sample, int index, double o, double[] w, double[] z)
        {
            var miB = SigmoidB(sample.Y, index);
            var gradient = -1 * (sample.Y - A2[index]) * miB * (1 - miB) * SigmoidB(sample.Y, index);

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

            P[index] -= gradientP * LearningRate;
            Q[index] -= gradientQ * LearningRate;
            R[index] -= gradientR * LearningRate;
        }

        #region File write methods

        public List<string> WriteFuzzySetRules(string fileName)
        {
            var lines = new List<string>();
            
            for (int i = 0; i < NumberOfRules; i++)
            {
                lines.Add("A1 = " + A1[i] + "\tB1 = " + B1[i] + "\tA2 = " + A2[i] + "\tB2 = " + B2[i]);
            }
            
            HelperFunctions.WriteToFile(fileName, lines);

            return lines;
        }

        public void WriteErrors(string fileName)
        {
            HelperFunctions.WriteToFile(fileName, ErrorsPerEpoch);
        }

        public void WriteOutputError(string fileName)
        {
            var lines = new List<string>();

            foreach (var sample in TrainingData)
            {
                var error = Math.Abs(CalculateF(sample.X, sample.Y) - sample.F);
                
                lines.Add(sample.X + ",\t" + sample.Y + ",\t" + error);
            }
            
            HelperFunctions.WriteToFile(fileName, lines);
        }

        #endregion
    }
}