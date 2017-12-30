namespace Anfis.TNorm
{
    public class ProductTNorm : ITNorm
    {
        public double Calculate(double x, double y)
        {
            return x * y;
        }
    }
}