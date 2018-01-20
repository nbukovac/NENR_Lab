using System;
using System.Collections.Generic;
using System.IO;

namespace GeneticANN.Helpers
{
    public static class HelperMethods
    {
        public static readonly Random Random = new Random();
        
        public static IEnumerable<string> ReadFromFile(string filePath)
        {
            var lines = new List<string>();

            using (var reader = new StreamReader(filePath))
            {
                var line = "";

                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        public static void WriteToFile(string filePath, IEnumerable<string> lines)
        {
            using (var writer = new StreamWriter(filePath)) 
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}