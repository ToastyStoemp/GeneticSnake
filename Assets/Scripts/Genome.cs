using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Genome : MonoBehaviour {

    //GLOBAL VARIABLES TEMP
    float enableChance = 0.25f;

    List<Neuron> NeuralNetwork;
    float fitness;

    public void Mate(ref Genome Partner, ref Genome OffSpring1, ref Genome OffSpring2)
    {
        List<int> InovationNumbersList = GetInovationList().Union(Partner.GetInovationList()).ToList();

        foreach (int inovation in InovationNumbersList)
        {
            Neuron myNeuron = GetNeuron(inovation);
            Neuron partnerNeuron = Partner.GetNeuron(inovation);

            //Dissabled Neurons have a chance to get enabled againg (Default value is 25% chance)
            if (myNeuron._enabled && Random.value < enableChance)
                myNeuron.EnableNeuron();
            if (partnerNeuron._enabled && Random.value < enableChance)
                partnerNeuron.EnableNeuron();

            if (myNeuron != null && partnerNeuron != null) //Both Genomes have the selected neuron, a random one will be selected, the oposing one is added to the other OffSpring
            {
                if (Random.value > 0.5f) //50% chance 
                {
                    OffSpring1.AddNeuron(myNeuron);
                    OffSpring2.AddNeuron(partnerNeuron);
                }
                else
                {
                    OffSpring1.AddNeuron(partnerNeuron);
                    OffSpring2.AddNeuron(myNeuron);
                }

            }
            else if (myNeuron != null)//the neuron is not present in the Partner, it's labeled as either disjoint or excess, it will be added to both
            {
                OffSpring1.AddNeuron(myNeuron);
                OffSpring2.AddNeuron(myNeuron);
            }
            else if (partnerNeuron != null)
            {
                OffSpring1.AddNeuron(partnerNeuron);
                OffSpring2.AddNeuron(partnerNeuron);
            }
        }
    }

    /// <summary>
    /// Adds a Neuron to the Neural Network 
    /// </summary>
    public bool AddNeuron(Neuron neuron)
    {
        if (GetInovationList().IndexOf(neuron._inovationNum) != -1)
        {
            NeuralNetwork.Add(neuron);
            return true;
        }
        return false;
    }
    /// <summary>
    /// Gets the neurons base on the inovation number
    /// </summary>
    public Neuron GetNeuron(int Inovation)
    {
        foreach (Neuron neuron in NeuralNetwork)
            if (neuron._inovationNum == Inovation)
                return neuron;

        return null;
    }
    /// <summary>
    /// returns the neuron count
    /// </summary>
    public int GetNeuronCount()
    {
        return NeuralNetwork.Count;
    }
    /// <summary>
    /// returns the neuron count with a list of inovation numbers to ignore
    /// </summary>
    public int GetNeuronCount(List<int> ignoreList)
    {
        int count = 0;
        foreach (Neuron neuron in NeuralNetwork)
            if (ignoreList.IndexOf(neuron._inovationNum) != -1)
                count++;
        return count;
    }
    /// <summary>
    /// returns a list of inovation numbers within the neuron
    /// </summary>
    public List<int> GetInovationList()
    {
        List<int> InovationList = new List<int>();
        foreach (Neuron neuron in NeuralNetwork)
            InovationList.Add(neuron._inovationNum);
        return InovationList;
    }
}
