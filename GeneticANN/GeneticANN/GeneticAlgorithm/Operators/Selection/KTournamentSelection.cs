using System.Collections.Generic;
using GeneticANN.GeneticAlgorithm.Populations;
using GeneticANN.Helpers;

namespace GeneticANN.GeneticAlgorithm.Operators.Selection
{
    public class KTournamentSelection<T> : ISelection<T>
    {
        private readonly int _k;

        public KTournamentSelection(int tournamentSize)
        {
            _k = tournamentSize;
        }
        
        public List<T> Select(Population<T> population)
        {
            var selected = new List<T>();

            while (selected.Count < _k)
            {
                var index = HelperMethods.Random.Next(population.PopulationSize);
                var chromosome = population[index];

                if (!selected.Contains(chromosome))
                {
                    selected.Add(chromosome);
                }
            }

            return selected;
        }
    }
}