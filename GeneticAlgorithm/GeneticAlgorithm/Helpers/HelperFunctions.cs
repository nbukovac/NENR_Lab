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
    }
}