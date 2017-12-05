using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Helpers;

namespace GeneticAlgorithm.Population
{
    public class Population<T> : IEnumerable<T>
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
            return PopulationList.GetEnumerator();
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

        public int Count()
        {
            return PopulationList.Count;
        }
        
        public void Add(T element)
        {
            PopulationList.Add(element);
        }
        
        public void Remove(T element)
        {
            PopulationList.Remove(element);
        }
    }
}