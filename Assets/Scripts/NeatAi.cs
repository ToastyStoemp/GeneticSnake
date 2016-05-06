using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NeatAi {

    public int poolSize,
               inputCount,
               outputCount;

    int generationCount = 0;
    int genomeCount = 0;

    List<Generation> memory;

    List<float> desired;

    List<float> input;

    public void Instantiate (List<float> Desired, List<float> Input, int poolSize) {
        memory = new List<Generation>();

        inputCount = Input.Count;
        outputCount = Desired.Count;

        Generation tempGen = new Generation();
        tempGen.Instantiate(generationCount, poolSize, inputCount, outputCount);
        memory.Add(tempGen);

        desired = Desired;
        input = Input;
    }
	
	public List<float> Tick()
    {
        memory[generationCount].pool[genomeCount].SetInputs(input);
        memory[generationCount].pool[genomeCount].Calculate();
        return memory[generationCount].pool[genomeCount].GetOutputs();
    }

	public void Mutate()
	{
		memory [generationCount].pool [genomeCount].Mutate ();
	}

	public void Print(int generationNum, int genomeNum, Vector3 pos)
	{
		if (Application.isPlaying) {
			memory [generationNum].pool [genomeNum].Print (pos);
			
		}
	}
}

//first iteration -> test fitness
//Check fitness ranking
//Check diversity ranking ~
//combine ranks ~
//Decide probablity
//Crossover
//Mutation
//Copy to new generation
