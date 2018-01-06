using System;

namespace Anfis.Helpers
{
    public static class Constants
    {
        public const int XLowerBound = -4;
        public const int XUpperBound = 4;
        public const int YLowerBound = -4;
        public const int YUpperBound = 4;

        public static readonly Random Random = new Random();

        public const double ArrayLowerBound = -1;
        public const double ArrayUpperBound = 1;

        public const string ResultsFolder = "/Results/";
    }
}