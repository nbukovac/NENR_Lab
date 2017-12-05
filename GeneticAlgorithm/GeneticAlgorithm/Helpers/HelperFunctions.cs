using System;

namespace GeneticAlgorithm.Helpers
{
    public class HelperFunctions
    {
        public static readonly Random Random = new Random();

        public static void CheckIndex(int index, int listSize)
        {
            if (index < 0 || index >= listSize)
            {
                throw new IndexOutOfRangeException();
            }
        }

        public static decimal GenerateRandomValue(decimal min, decimal max)
        {
            return (decimal)Random.NextDouble() * (max - min) + min;
        }

        public static decimal[] GetArray(decimal arrayValue, int size)
        {
            var array = new decimal[size];

            for (int i = 0; i < size; i++)
            {
                array[i] = arrayValue;
            }

            return array;
        }
    }
}