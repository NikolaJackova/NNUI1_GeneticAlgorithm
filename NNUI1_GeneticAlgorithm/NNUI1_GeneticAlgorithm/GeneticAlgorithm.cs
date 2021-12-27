using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNUI1_GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        public Gene[] InitialPubs { get; set; }
        private Population CurrentPopulation { get; set; }
        public Fitness Fitness { get; set; }

        private static readonly Random random = new Random();
        public GeneticAlgorithm()
        {
            InitialPubs = ExcelReader.GetPubListFromExcel();
            Fitness = new Fitness();
        }
        public Gene[] RunGeneticAlgorithm()
        {
            Population population = CreatePopulation();
            for (int i = 0; i < Constants.MaxPopulations; i++)
            {
                population = NextGeneration(population);
                RankRoutes(population);
            }
            return population.GetBestRoute().Genes;
        }

        private Population NextGeneration(Population population)
        {
            Population matingPool = CreateMatingPool(population);
            Population children = BreedPopulation(matingPool);
            MutatePopulation(children);
            return children;
        }

        private Population CreatePopulation()
        {
            Individual[] routes = new Individual[Constants.PopulationSize];
            for (int i = 0; i < Constants.PopulationSize; i++)
            {
                routes[i] = CreateRoute();
            }
            Population newPopulation = new Population(routes);
            RankRoutes(newPopulation);
            return newPopulation;
        }
        private Individual CreateRoute()
        {
            Individual route = new Individual(InitialPubs);
            route.ShufflePubs();
            return route;
        }
        private void RankRoutes(Population population)
        {
            foreach (var route in population.Individuals)
            {
                route.FitnessValue = Fitness.CalculateDistance(route);
            }
        }
        public Population CreateMatingPool(Population population)
        {
            Individual[] routes = new Individual[population.Individuals.Length];

            double cumulativeSum = population.Individuals.Sum(route => route.FitnessValue);
            double cumulativeWeight = 0;
            foreach (var route in population.Individuals)
            {
                route.WeightedFitnessValue = route.FitnessValue / cumulativeSum + cumulativeWeight;
                cumulativeWeight += route.FitnessValue / cumulativeSum;
            }
            Individual[] elite = population.GetEliteIndividual();
            for (int i = 0; i < Constants.EliteSize; i++)
            {
                routes[i] = elite[i];
            }
            for (int i = Constants.EliteSize; i < Constants.PopulationSize; i++)
            {
                double randomNumber = random.NextDouble();
                for (int j = 0; j < Constants.PopulationSize; j++)
                {
                    if (j+1 < Constants.PopulationSize)
                    {
                        if (randomNumber > population.Individuals[j].WeightedFitnessValue && randomNumber <= population.Individuals[j + 1].WeightedFitnessValue)
                        {
                            routes[i] = new Individual(population.Individuals[j].Genes);
                            break;
                        }
                    }
                    else
                    {
                        routes[i] = new Individual(population.Individuals[j].Genes);
                    }
                }
            }
            return new Population(routes);
        }
        public Individual Breed(Individual parent1, Individual parent2)
        {
            int genA = random.Next(0, parent1.Genes.Length - 1);
            int genB = random.Next(0, parent2.Genes.Length);

            int startGene = Math.Min(genA, genB);
            int endGene = Math.Max(genA, genB);

            Gene[] children = new Gene[Constants.IndividualSize];

            int indexInChild = endGene;
            for (int i = startGene; i < endGene; i++)
            {
                children[i] = parent1.Genes[i];
            }
            for (int i = endGene; i < parent2.Genes.Length; i++)
            {
                if (!children.Contains(parent2.Genes[i]))
                {
                    children[indexInChild] = parent2.Genes[i];
                    if (indexInChild == parent2.Genes.Length - 1)
                    {
                        indexInChild = 0;
                    }
                    else
                    {
                        indexInChild++;
                    }
                }
            }
            for (int i = 0; i < endGene; i++)
            {
                if (!children.Contains(parent2.Genes[i]))
                {
                    children[indexInChild] = parent2.Genes[i];
                    if (indexInChild == parent2.Genes.Length - 1)
                    {
                        indexInChild = 0;
                    }
                    else
                    {
                        indexInChild++;
                    }
                }
            }
            return new Individual(children);
        }
        public Population BreedPopulation(Population matingPool)
        {
            Individual[] routes = new Individual[matingPool.Individuals.Length];
            for (int i = 0; i < Constants.EliteSize; i++)
            {
                routes[i] = matingPool.Individuals[i];
            }
            for (int i = Constants.EliteSize; i < matingPool.Individuals.Length; i++)
            {
                Individual children = Breed(matingPool.Individuals[i], matingPool.Individuals[matingPool.Individuals.Length - i - 1]);
                routes[i] = children;
            }
            return new Population(routes);
        }
        public void Mutate(Individual route)
        {
            for (int i = 0; i < route.Genes.Length; i++)
            {
                if (random.NextDouble() < Constants.MutationRate)
                {
                    int swappedWith = random.Next(0, route.Genes.Length);

                    Gene city1 = route.Genes[i];
                    Gene city2 = route.Genes[swappedWith];

                    route.Genes[i] = city2;
                    route.Genes[swappedWith] = city1;
                }
            }
        }
        public void MutatePopulation(Population population)
        {
            for (int i = 0; i < population.Individuals.Length; i++)
            {
                Mutate(population.Individuals[i]);
            }
        }
    }
}
