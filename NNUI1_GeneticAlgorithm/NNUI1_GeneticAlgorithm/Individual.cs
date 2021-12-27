using System;
using System.Linq;

namespace NNUI1_GeneticAlgorithm
{
    public class Individual
    {
        public Gene[] Genes { get; set; }
        public double FitnessValue { get; set; }
        public double WeightedFitnessValue { get; set; }

        private static readonly Random random = new Random();
        public Individual(Gene[] pubs)
        {
            Genes = pubs;
        }
        public void ShufflePubs()
        {
            Genes = Genes.OrderBy(pub => random.Next()).ToArray();
        }
        public override string ToString()
        {
            string result = "";
            foreach (var pub in Genes)
            {
                result += pub.ToString() + "\n";
            }
            return result;
        }
    }
}
