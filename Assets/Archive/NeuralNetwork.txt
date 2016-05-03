using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeuralNetwork : MonoBehaviour {

    int inputCount;
    int outputCount;
    int hiddenLayerCount;
    //float bias; Bias will be ignored for now

    List<NeuralLayer> Layers;
    List<NeuralLayer> WeightLayers;

    public NeuralNetwork(int InputCount, int OutputCount, int HiddenLayerCount)//, float Bias) //4 //3 //1
    {
        inputCount = InputCount;
        outputCount = OutputCount;
        hiddenLayerCount = HiddenLayerCount;
        //bias = Bias;

        Layers = new List<NeuralLayer>();
        WeightLayers = new List<NeuralLayer>();

        //Initialize layers  Only works for 1 hidden layer
        Layers.Add(new NeuralLayer(InputCount, 1));
        Layers.Add(new NeuralLayer(InputCount + OutputCount, 1));
        Layers.Add(new NeuralLayer(OutputCount, 1));

        WeightLayers.Add(new NeuralLayer(InputCount + OutputCount, InputCount));
        WeightLayers.Add(new NeuralLayer(OutputCount, InputCount + OutputCount));
    }

    public void SetInputs(List<float> inputs)
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            Layers[0].SetCell(i, 0, inputs[i]);
        }
    }

    public void AddWeightLayer(NeuralLayer weightLayer, int index)
    {
        WeightLayers[index] = weightLayer;
    }

    public NeuralLayer CreateRandomWeightLayer(int incommingCount, int outgoingCount) // Set random weights the weight layers ( only used once at the start of the AI ) 
    {
        NeuralLayer weightLayer = new NeuralLayer(outgoingCount, incommingCount);
        weightLayer.SetRandom(0, 1);
        return weightLayer;
    }

    public List<float> GetOutPuts()
    {
        Calculate(); //Calculate everything

        List<float> outputs = new List<float>();
        NeuralLayer OutPutLayer = Layers[Layers.Count - 1];
        for (int i = 0; i < OutPutLayer._rows; i++)
        {
            outputs.Add(OutPutLayer.GetCell(i, 0)); //Take only the first row of the outputs ( should be only 1 row ), false makes sure it does not apply the weight;
        }
        return outputs;
    }

    private void Calculate()
    {
        int weightIndex = 0;
        for (int i = 0; i < Layers.Count - 1; i++)
        {
            NeuralLayer InputLayer = Layers[i];
            NeuralLayer OutputLayer = new NeuralLayer(WeightLayers[weightIndex]._rows, Layers[i]._columns);
            WeightLayers[weightIndex].Mul(ref InputLayer, ref OutputLayer);

            Layers[i + 1] = OutputLayer;
            weightIndex++;
        }
    }

    public NeuralLayer CreateLayer(int count)
    {
        return new NeuralLayer(count, 1); //All layers are 1 column Matrixes for now.
    }

    public Genome ToGenome()
    {
        Genome resultGenome = new Genome();
        List<float> weights = new List<float>();
        for (int i = 0; i < WeightLayers.Count; i++)
        {
            for (int k = 0; k < WeightLayers[i]._rows; k++)
            {
                for (int l = 0; l < WeightLayers[i]._columns; l++)
                {
                    weights.Add(WeightLayers[i].GetCell(k, l));
                }
            }
        }
        resultGenome.weights = weights;
        return resultGenome;
    }

    public void fromGenome(Genome Genome) //does not work if more then one hidden layer!!!
    {
        int index = 0;
        for (int i = 0; i < WeightLayers.Count; i++)
        {
            for (int r = 0; r < WeightLayers[i]._rows; r++)
            {
                for (int c = 0; c < WeightLayers[i]._columns; c++)
                {
                    WeightLayers[i].SetCell(r, c, Genome.weights[index++]);
                }
            }
        }
    }

}