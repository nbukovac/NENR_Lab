using System;

namespace Anfis.TransferFunction
{
    public class SigmoidalTransferFunction : ITransferFunction
    {
        public double Calculate(double x, double a, double b)
        {
            return 1 / (1 + Math.Exp(b * (x - a)));
        }
    }
}