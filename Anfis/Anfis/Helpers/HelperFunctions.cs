using System;
using System.Collections.Generic;
using System.IO;

namespace Anfis.Helpers
{
    public static class HelperFunctions
    {
        public static double[] ArrayInitializer(int arraySize, bool randomize = true)
        {
            var array = new double[arraySize];

            if (!randomize)
            {
                return array;
            }

            for (var i = 0; i < arraySize; i++)
            {
                array[i] = Constants.Random.NextDouble() * (Constants.ArrayUpperBound - Constants.ArrayLowerBound) +
                           Constants.ArrayLowerBound;
            }

            return array;
        }

        public static void WriteToFile(string filePath, IEnumerable<string> lines)
        {
            using (var outputStream = new StreamWriter(Environment.CurrentDirectory + Constants.ResultsFolder + filePath))
            {
                foreach (var line in lines)
                {
                    outputStream.WriteLine(line);
                }
            }
        }
    }
}