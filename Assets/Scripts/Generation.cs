using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Generation {

    float DeltaDisjoint = 2.0f;
    float DeltaWeights = 0.4f;
    float DeltaThreshold = 1.0f;

    int _index;
    int _poolSize;
    int _inputCount;
    int _outputCount;

    public List<Genome> pool = new List<Genome>();

    public Generation Instantiate(int index, int poolSize, int inputCount, int outputCount)
    {
        _index = index;
        _poolSize = poolSize;
        _inputCount = inputCount;
        _outputCount = outputCount;
        return this;
    }

    public void FillPool()
    {
        //Create a new pool of Genomes
        pool = new List<Genome>();
        for (int i = 0; i < _poolSize; i++)
        {
            Genome tempGenome = new Genome(_index, i, _inputCount, _outputCount);
            tempGenome.CreateBasicNodes();
            tempGenome.CreateRandomConnection();
            pool.Add(tempGenome);
        }
    }

	public void SetInputs(List<float> inputs)
	{
		foreach (Genome genome in pool) {
			genome.SetInputs (inputs);
		}
	}

	public List<float> GetOutputs(int genomeNum)
	{
		return pool [genomeNum].GetOutputs ();
	}

	public Genome GetFittestGenome()
	{
		Genome fittest = new Genome (0, 0, 0, 0);
		fittest._fitness = 0.0f;
		foreach (Genome genome in pool) {
			if (genome._fitness > fittest._fitness) {
				fittest = genome;
			}
		}
		return fittest;
	}

    public Genome GetProbabiliestGenome()
    {
        Genome Probabilitiest = new Genome(0, 0, 0, 0);
        Probabilitiest._probability = 0.0f;
        foreach (Genome genome in pool)
        {
            if (genome._probability > Probabilitiest._fitness)
            {
                Probabilitiest = genome;
            }
        }
        return Probabilitiest;
    }

    public void Calculate()
	{
		foreach (Genome genome in pool) {
			genome.Calculate ();
		}
	}

	public void CalcFitness(List<float> desired)
	{
		foreach (Genome genome in pool) {
			genome.FitnessCalc (desired);
		}
	}

    public void CalcProbablity()
    {
        foreach (Genome genome in pool)
        {
            //genome.ProbabilityCalc();
        }
    }

    public void RankGenomes()
    {
		float PC = 1.0f / _poolSize;
        pool.Sort((x, y) => x._fitness.CompareTo(y._fitness));
        pool.Reverse();
		pool [0]._probability = PC;
		for (int i = 1; i < pool.Count; ++i) {
			pool [i]._probability = Mathf.Pow((1 - PC),i) * PC;
		}
    }

    public List<Genome> Selection()
    {
        List<Genome> result = new List<Genome>();
        result.Add(pool[0]);
        while (result.Count < _poolSize / 2)
        {
            int randomIndex = (int)Mathf.Floor(Random.value * _poolSize);
            if (Random.Range(0.0f, 1.0f / _poolSize) < pool[randomIndex]._probability)
            {
                if (!result.Contains(pool[randomIndex]))
                {
                    result.Add(pool[randomIndex]);
                }
            }
        }
        return result;
    }

    public List<Genome> FillNewGeneration(List<Genome> oldPool)
    {
        List<Genome> result = ObjectCopier.Clone < List<Genome>>(oldPool);

        for (int i = 0; i < result.Count; i++)
        {
            result[i]._index = i;
        }
        for (int i = 0; i < oldPool.Count; i+=2)
        {
            Genome Offspring1 = new Genome(_index, i + oldPool.Count, oldPool[i]._inputCount, oldPool[i]._outputCount);
            Genome Offspring2 = new Genome(_index, i + 1 + oldPool.Count, oldPool[i]._inputCount, oldPool[i]._outputCount);
            oldPool[i].Mate(oldPool[i+1], Offspring1, Offspring2);
            Offspring1.CreateBasicNodes();
            Offspring1.CreateExtendedNodes();
            Offspring2.CreateBasicNodes();
            Offspring2.CreateExtendedNodes();
            result.Add(Offspring1);
            result.Add(Offspring2);
        }
        return result;
    }

    public void SetGeneration(List<Genome> newPool)
    {
        pool = new List<Genome>(newPool);
        foreach(Genome genome in pool)
        {
            genome._genIndex = _index;
        }
    }

    public void Mutate()
    {
        foreach (Genome gen in pool)
        {
            gen.Mutate();
        }
    }

    public Genome GetLargestGenome()
    {
        Genome largest = new Genome(0, 0, 0, 0);
        foreach(Genome gen in pool)
        {
            if (gen.NodeCollection.Count > largest.NodeCollection.Count)
            {
                largest = gen;
            }
        }
        return largest;
    }

    public bool sameSpecies(Genome Genome1, Genome Genome2)
    {
        float diversity = DeltaDisjoint * Genome1.DisJoint(Genome2);
        float weightDiversity = DeltaWeights * Genome1.WeightsDifference(Genome2);
        return diversity + weightDiversity < DeltaThreshold;
    }
}