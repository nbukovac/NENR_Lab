using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Helpers;

namespace GeneticAlgorithm.Population
{
    public class Population<T> : IEnumerable<T> where T : DecimalArrayChromosome
    {
        public List<T> PopulationList { get; set; }
        public int Size { get; }

        public Population(int populationSize)
        {
            PopulationList = new List<T>(populationSize);
            Size = populationSize;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)PopulationList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T GetAtIndex(int index)
        {
            HelperFunctions.CheckIndex(index, PopulationList.Count);
            
            return PopulationList.ElementAt(index);
        }

        public void Add(T element)
        {
            PopulationList.Add(element);
        }
        
        public void RemoveAtIndex(int index)
        {
            HelperFunctions.CheckIndex(index, PopulationList.Count);

            PopulationList.RemoveAt(index);
        }
        
        public T GetBest()
        {
            return PopulationList.OrderBy(x => x.Fitness).First();
        }

        public T GetWorst()
        {
            return PopulationList.OrderBy(x => x.Fitness).Last();
        }
    }
}