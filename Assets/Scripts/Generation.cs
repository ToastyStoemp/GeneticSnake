using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Generation {


    float DeltaDisjoint = 2.0f;
    float DeltaWeights = 0.4f;
    float DeltaThreshold = 1.0f;

    int generationIndex;

    public List<Genome> pool = new List<Genome>();

    public Generation Instantiate(int index, int poolSize, int inputCount, int outputCount)
    {
        generationIndex = index;

        //Create a new pool of Genomes
        pool = new List<Genome>();
        for (int i = 0; i < poolSize; i++)
        {
            pool.Add(new Genome(i * generationIndex, inputCount, outputCount));
        }
        return this;
    }

    public void Rank()
    {
        pool.Sort((x, y) => x._fitness.CompareTo(y._fitness));
    }

    /// <summary>
    /// The entire sequence of executions of 1 generation
    /// </summary>


    public bool sameSpecies(Genome Genome1, Genome Genome2)
    {
        float diversity = DeltaDisjoint * Genome1.DisJoint(Genome2);
        float weightDiversity = DeltaWeights * Genome1.WeightsDifference(Genome2);
        return diversity + weightDiversity < DeltaThreshold;
    }
}