namespace Anfis.TransferFunction
{
    public interface ITransferFunction
    {
        double Calculate(double x, double a, double b);
    }
}