using System;
using GeneticAlgorithm.Population;

namespace GeneticAlgorithm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var population = new Population<DecimalArrayChromosome>(6);
        }
    }
}