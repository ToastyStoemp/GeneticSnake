using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//first iteration -> test fitness
//Check fitness ranking
//Check diversity ranking
//combine ranks
//Decide probablity
//Crossover
//Mutation
//Copy to new generation

public class Generation : MonoBehaviour {


    float DeltaDisjoint = 2.0f;
    float DeltaWeights = 0.4f;
    float DeltaThreshold = 1.0f;

    int generationIndex;

    List<Genome> pool = new List<Genome>();

    public void Instantiate(int index, int poolSize, int inputCount, int outputCount)
    {
        generationIndex = index;

        //Create a new pool of Genomes
        pool = new List<Genome>();
        for (int i = 0; i < poolSize; i++)
        {
            pool.Add(new Genome(i * generationIndex, inputCount, outputCount));
        }
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