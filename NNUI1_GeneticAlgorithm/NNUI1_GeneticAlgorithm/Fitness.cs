using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNUI1_GeneticAlgorithm
{
    public class Fitness
    {
        public double CalculateDistance(Individual route)
        {
            double pathDistance = 0;
            for (int i = 0; i < route.Genes.Length - 1; i++)
            {
                pathDistance += route.Genes[i].CountDistance(route.Genes[i + 1]);
            }
            return 1 / pathDistance;
        }
    }
}
