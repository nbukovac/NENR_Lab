using System.Collections;
using System.Collections.Generic;

namespace GeneticANN.GeneticAlgorithm.Populations
{
    public class Population<T> : IEnumerable<T>
    {
        public List<T> Chromosomes { get; }
        public int PopulationSize { get; set; }
        
        public Population(int populationSize)
        {
            Chromosomes = new List<T>();
            PopulationSize = populationSize;
        }
        
        public T this[int i] => Chromosomes[i];

        public void AddChromosome(T chromosome)
        {
            Chromosomes.Add(chromosome);
        }

        public void RemoveChromosome(T chromosome)
        {
            Chromosomes.Remove(chromosome);
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return Chromosomes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}