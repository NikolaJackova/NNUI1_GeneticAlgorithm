using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNUI1_GeneticAlgorithm
{
    public class Population
    {
        public Individual[] Individuals { get; set; }

        public Population(Individual[] individuals)
        {
            Individuals = individuals;
        }

        public Individual GetBestRoute()
        {
            double maxFitness = Individuals.Max(individual => individual.FitnessValue);
            return Individuals.First(item => item.FitnessValue == maxFitness);
        }
        public Individual[] GetEliteIndividual()
        {
            Individuals = Individuals.OrderByDescending(route => route.FitnessValue).ToArray();
            Individual[] eliteIndividuals = new Individual[Constants.EliteSize];
            for (int i = 0; i < Constants.EliteSize; i++)
            {
                eliteIndividuals[i] = Individuals[i];
            }
            return eliteIndividuals;
        }

    }
}
