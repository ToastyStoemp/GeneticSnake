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

        //initialize the layers

    }

    public void SetInputs(List<float> inputs)
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            Layers[0].SetCell(i, 0, inputs[i]);
        }
    }

    public void AddWeightLayer(NeuralLayer weightLayer)
    {
        WeightLayers.Add(weightLayer);
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

}