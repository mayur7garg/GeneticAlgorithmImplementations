using System;

public class TS_DNA
{
    public int[] Genes { get; private set; }
    public float Fitness { get; private set; }

    private Random random;
    private Func<int, float> fitnessFunction;

    public TS_DNA(int size, Random random, Func<int, float> fitnessFunction, bool shouldInitGenes = true)
    {
        Genes = new int[size];
        this.random = random;
        this.fitnessFunction = fitnessFunction;

        if (shouldInitGenes)
        {
            for (int i = 0; i < size; i++)
            {
                Genes[i] = i;
            }
            Genes = ShuffleGenes(Genes);
        }
    }
    int[] ShuffleGenes(int[] geneArray)
    {
        int[] shuffledGenes = new int[geneArray.Length];

        int randNo;
        for (int i = geneArray.Length; i>=1; i--)
        {
            randNo = random.Next(0, i);
            shuffledGenes[i - 1] = geneArray[randNo];
            geneArray[randNo] = geneArray[i - 1];
        }
        return shuffledGenes;
    }
    public float CalculateFitness(int index)
    {
        Fitness = fitnessFunction(index);
        return Fitness;
    }

    public TS_DNA Crossover(TS_DNA otherParent)
    {
        TS_DNA child = new TS_DNA(Genes.Length, random, fitnessFunction, shouldInitGenes: false);

        //Implement Crossover here

        for (int i = 0; i < Genes.Length; i++)
        {
            child.Genes[i] = this.Genes[i];
        }
        child.Mutate((float)random.NextDouble());
        return child;
    }

    public void Mutate(float mutationRate)
    {
        if (random.NextDouble()<mutationRate)
        {
            int gene1 = 0;
            int gene2 = 0;
            gene1 = random.Next(Genes.Length);
            do
            {
                gene2 = random.Next(Genes.Length);
            } while (gene1==gene2);

            int temp = Genes[gene1];
            Genes[gene1] = Genes[gene2];
            Genes[gene2] = temp;

        }
    }
}
