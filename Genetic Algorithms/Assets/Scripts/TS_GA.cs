using System;
using System.Collections.Generic;

public class TS_GA
{
    public List<TS_DNA> Population { get; private set; }
    public int Generation { get; private set; }
    public float BestFitness { get; private set; }
    public int[] BestGenes { get; private set; }

    public int Elitism;
    public float MutationRate;
    private float fitnessSum;
    private Random random;
    private List<TS_DNA> newPopulation;

    public TS_GA(int populationSize, int dnaSize, Random random, Func<int, float> fitnessFunction, int elitism, float mutationRate = 0.01f)
    {
        Generation = 1;
        Elitism = elitism;
        MutationRate = mutationRate;
        Population = new List<TS_DNA>(populationSize);
        newPopulation = new List<TS_DNA>(populationSize);
        this.random = random;
        BestGenes = new int[dnaSize];

        for (int i = 0; i < populationSize; i++)
        {
            Population.Add(new TS_DNA(dnaSize, random, fitnessFunction, shouldInitGenes: true));
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
            if (i < Elitism)
            {
                newPopulation.Add(Population[i]);
            }
            else
            {
                TS_DNA parent1 = ChooseParent();
                TS_DNA parent2 = ChooseParent();

                TS_DNA child = parent1.Crossover(parent2);
                child.Mutate(MutationRate);

                newPopulation.Add(child);
            }
        }
        List<TS_DNA> tempList = Population;
        Population = newPopulation;
        newPopulation = tempList;

        Generation++;
    }

    private void CalculateFitness()
    {
        fitnessSum = 0f;
        TS_DNA bestDNA = Population[0];

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

    private TS_DNA ChooseParent()
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

    public int CompareDNA(TS_DNA a, TS_DNA b)
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
