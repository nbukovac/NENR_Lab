using System;
using System.Collections;
using System.Collections.Generic;
using GeneticANN.Helpers;

namespace GeneticANN.Dataset
{
    public class Dataset : IEnumerable<Sample>
    {
        public IList<Sample> Samples { get; set; }

        public Dataset(string datasetFilePath)
        {
            Samples = new List<Sample>();
            
            ParseDatasetFromFile(datasetFilePath);
        }

        private void ParseDatasetFromFile(string datasetFilePath)
        {
            var lines = HelperMethods.ReadFromFile(datasetFilePath);

            foreach (var line in lines)
            {
                var split = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                var sample = new Sample(new string[] {split[0], split[1]}, new string[] { split[2], split[3], split[4] });
                Samples.Add(sample);
            }
        }

        public int Count()
        {
            return Samples.Count;
        }

        public IEnumerator<Sample> GetEnumerator()
        {
            return Samples.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}