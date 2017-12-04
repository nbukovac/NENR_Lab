using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Helpers;
using GeneticAlgorithm.Population;

namespace GeneticAlgorithm.Operators.Selection
{
    public class KTournamentSelection : ISelection<DecimalArrayChromosome>
    {
        private readonly int _k;

        public KTournamentSelection(int k)
        {
            _k = k;
        }
        
        public List<DecimalArrayChromosome> Select(Population<DecimalArrayChromosome> population)
        {
            var selected = new List<DecimalArrayChromosome>();

            while (selected.Count < _k)
            {
                var index = HelperFunctions.Random.Next(population.Size);
                var chromosome = population.GetAtIndex(index);

                if (!selected.Contains(chromosome))
                {
                    selected.Add(chromosome);
                }
            }

            return selected;
        }
    }
}