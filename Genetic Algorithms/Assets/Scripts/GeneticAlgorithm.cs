using System;
using System.Collections.Generic;

public class GeneticAlgorithm<T>
{
    public List<DNA<T>> Population { get; private set; }
    public int Generation { get; private set; }
    public float BestFitness { get; private set; }
    public T[] BestGenes { get; private set; }

    public int Elitism;
    public float MutationRate;
    private float fitnessSum;
    private Random random;
    private List<DNA<T>> newPopulation;

    public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction, int elitism, float mutationRate = 0.01f)
    {
        Generation = 1;
        Elitism = elitism;
        MutationRate = mutationRate;
        Population = new List<DNA<T>>(populationSize);
        newPopulation = new List<DNA<T>>(populationSize);
        this.random = random;
        BestGenes = new T[dnaSize];

        for (int i = 0; i < populationSize; i++)
        {
            Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
        }
    }

    public void NewGeneration()
    {
        if (Population.Count <= 0)
        {
            return;
        }

        CalculateFitness();
        Population.Sort(CompareDNA);
        newPopulation.Clear();

        for (int i = 0; i < Population.Count; i++)
        {
            if (i<Elitism)
            {
                newPopulation.Add(Population[i]);
            }
            else
            {
                DNA<T> parent1 = ChooseParent();
                DNA<T> parent2 = ChooseParent();

                DNA<T> child = parent1.Crossover(parent2);
                child.Mutate(MutationRate);

                newPopulation.Add(child);
            }
        }
        List<DNA<T>> tempList = Population;
        Population = newPopulation;
        newPopulation = tempList;

        Generation++;
    }

    private void CalculateFitness()
    {
        fitnessSum = 0f;
        DNA<T> bestDNA = Population[0];

        for (int i = 0; i < Population.Count; i++)
        {
            fitnessSum += Population[i].CalculateFitness(i);
            if (Population[i].Fitness > bestDNA.Fitness)
            {
                bestDNA = Population[i];
            }
        }
        BestFitness = bestDNA.Fitness;
        bestDNA.Genes.CopyTo(BestGenes, 0);
    }

    private DNA<T> ChooseParent()
    {
        double randomNum = random.NextDouble() * fitnessSum;

        for (int i = 0; i < Population.Count; i++)
        {
            if (randomNum < Population[i].Fitness)
            {
                return Population[i];
            }
            randomNum -= Population[i].Fitness;
        }
        return null;
    }
    
    public int CompareDNA(DNA<T> a, DNA<T> b)
    {
        if (a.Fitness > b.Fitness)
        {
            return -1;
        }
        else if (a.Fitness < b.Fitness)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}