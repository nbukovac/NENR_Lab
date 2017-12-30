namespace Anfis.Helpers
{
    public class HelperFunctions
    {
        public static double[] ArrayInitializer(int arraySize)
        {
            var array = new double[arraySize];

            for (var i = 0; i < arraySize; i++)
            {
                array[i] = Constants.Random.NextDouble() * (Constants.ArrayUpperBound - Constants.ArrayLowerBound) +
                           Constants.ArrayLowerBound;
            }

            return array;
        }
    }
}