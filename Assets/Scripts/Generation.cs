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

    public void RankGenomes()
    {
		float PC = 0.8f;

        pool.Sort((x, y) => x._fitness.CompareTo(y._fitness));
		pool [0]._probability = PC;
		for (int i = 1; i < pool.Count - 1 ; i++) {
			//pool [i]._probability = Mathf.Exp ((1 - PC), (i - 1)) * PC;
		}
		pool.Sort((x, y) => x._probability.CompareTo(y._probability));
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