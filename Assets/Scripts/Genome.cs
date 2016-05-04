using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Genome : MonoBehaviour {

    //GLOBAL VARIABLES TEMP
    int globalGenomeCounter = 0;
    int globalInovationCounter = 0;

    float matingChance = 0.05f;
    float enableChance = 0.25f;
    float weightChangeChance = 0.8f;
    float perturbedChance = 0.9f;
    float perturbMaxChangeAmount = 0.3f; //HAS TO CHANGE DEPENDING ON TESTS! Smaller might take longer to get to optimal vallues, higher might result in skipping good vallues perhaps apply inverse sigmoid function
    float randomWeightChance = 0.1f; // has to be 1 - perturbedChance
    float newNodeChance = 0.01f;
    float newConnectionChance = 0.1f;
    

    List<Neuron> NeuralNetwork;
    List<Node> NodeCollection;
    float _fitness;
    int _genomeIndex;

    int _inputCount;
    int _outputCount;

    public Genome(int index, int inputCount, int outputCount)
    {
        _genomeIndex = index;
        _inputCount = inputCount;
        _outputCount = outputCount;

        //Make the start input and output nodes
        List<Node> inputNodes = new List<Node>();
        for (int i = 0; i < inputCount; i++)
        {
            inputNodes.Add(new Node(i, NeuronType.Input));
        }
        List<Node> outputNodes = new List<Node>();
        for (int i = 0; i < outputCount; i++)
        {
            outputNodes.Add(new Node(inputCount + i, NeuronType.OutPut));
        }

        //Generate a random connection to a random OutPut node
        globalInovationCounter++;
        Node from = inputNodes[(int)Random.Range(0, inputNodes.Count)];
        Node to = outputNodes[(int)Random.Range(0, outputNodes.Count)];
        NeuralNetwork.Add(new Neuron(from._nodeIndex, to._nodeIndex, Random.Range(-1, 1), globalInovationCounter));

        //Merge the nodes into one list 
        NodeCollection = inputNodes.Union(outputNodes).ToList();
    }
    /// <summary>
    /// Set the input data for the AI
    /// </summary>
    public void SetInputs(List<float> inputList)
    {
        if (inputList.Count != _inputCount)
        {
            Debug.Log("Not enough inputs given!");
            return; 
        }

        //Since we added the Inputs first, we can just access them through the index no need for a seperate list ( technically the nodeType is also not necesary )
        for (int i = 0; i < _inputCount; i++)
        {
            NodeCollection[i]._value = inputList[i];
        }
    }
    /// <summary>
    /// After running the calculations throughout all the nodes we can obtain the outputs.
    /// </summary>
    public List<float> GetInputs()
    {
        List<float> results = new List<float>();

        //Since we added the outputs after the inputs the indexes are easily found
        for (int i = 0; i < _outputCount; i++)
        {
            results.Add(NodeCollection[i + _inputCount]._value);
        }
        return results;
    }
    /// <summary>
    /// Calculates the end values for every node
    /// </summary>
    public void Calculate()
    {
        foreach (Neuron connection in NeuralNetwork)
        {
            if (connection._enabled) //Make sure to process only the enabled networks
            {
                Node inputNode = NodeCollection[connection._in];
                Node targetNode = NodeCollection[connection._out];

                targetNode._value += inputNode._value * connection._weight;
            }
        }
    }
    /// <summary>
    /// Reseting all the values ( in case of copy ), safety measure
    /// </summary>
    public void Reset()
    {
        //Reseting all the values ( in case of copy ), safety measure
        foreach(Node node in NodeCollection)
        {
            node._value = 0.0f;
        }
    }
    /// <summary>
    /// Mates/combines 2 genomes into 2 different OffSpring
    /// </summary>
    public void Mate(ref Genome Partner, ref Genome OffSpring1, ref Genome OffSpring2)
    {
        List<int> InovationNumbersList = GetInovationList().Union(Partner.GetInovationList()).ToList();

        foreach (int inovation in InovationNumbersList)
        {
            Neuron myNeuron = GetNeuron(inovation);
            Neuron partnerNeuron = Partner.GetNeuron(inovation);

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
    /// Alters the current Genome with the different mutation steps
    /// </summary>
    public void Mutate()
    {
        //Some parameters for debugging and informatics
        int newNodeCounter = 0;
        int newConnectionCounter = 0;
        int weightsChanged = 0;
        int weightsPertured = 0;
        int weightsRandomized = 0;
        int connectionsEnabled = 0;

        List<Neuron> ToAddNeurons = new List<Neuron>(); //Not needed for all programming languages, this makes sure that newly added neurons are not subject to mutation in this generation yet.

        //Node Mutation is handled first, to prevent the same issues as adding Neuron connections while looping over the connections
        foreach (Node node in NodeCollection)
        {
            //Chance for new connection
            if (Random.value < newConnectionChance) //chance for it to spawn a new connection (default is 10%)
            {
                Node startNode = GetRandomNode();
                Node endNode = GetRandomNode(startNode._nodeIndex);

                globalInovationCounter++;
                Neuron newConnection = new Neuron(startNode._nodeIndex, endNode._nodeIndex, Random.Range(-1, 1), globalInovationCounter); //Set the weight to be random
                ToAddNeurons.Add(newConnection);
            }
        }

        //Connection Mutation
        foreach (Neuron neuron in NeuralNetwork)
        {
            //Dissabled Neurons have a chance to get enabled againg (Default value is 25%)
            if (neuron._enabled && Random.value < enableChance)
            {
                neuron._enabled = true;
                connectionsEnabled++;
            }

            //There is a high percentage that the weights of a neuron is changed (Default value is 80%)
            if (Random.value < weightChangeChance)
            {
                weightsChanged++;
                /*2 types of mutation
                Perturbed With the highest chance of occuring ( Default value is 90% ) */
                if (Random.value < perturbedChance)
                {
                    neuron._weight *= perturbMaxChangeAmount * Random.Range(-1, 1);
                    weightsPertured++; //DEBUG
                }
                else //Randomized, the weight is set to a random value ( Default value is 10% )
                {
                    neuron._weight = Random.Range(-1, 1);
                    weightsRandomized++; //DEBUG
                }
            }

            //New Neuron with protection
            if (Random.value < newNodeChance) //chance for it to spawn a new node (default value is 1%)
            {
                /*Create a new node, set the input and output. After adding a new Neuron to the network, it is bound to drop in fitness, 
                to give it some breathing room, we set the weight of the new neuron to the weight of the previous connection
               The previous connection is dissabled, and a new connection from the origin to this node is set with a weight of 1. 
               This will result in fitness close to fitness achieved in the previous generation */
                Node mutationNode = new Node(NodeCollection.Count + 1);
                newNodeCounter++; //DEBUG
                //Make apropriate connections
                int input = neuron._in; //this will be the input for the new node
                int output = neuron._out; //this will be the output for the new node
                globalInovationCounter++; //Increment the inovation number, for future tracking such ass diversity calculations
                Neuron inputConnection = new Neuron(input, mutationNode._nodeIndex, 1.0f, globalInovationCounter); //Set the weight to 1.0f to protect the new node
                globalInovationCounter++;
                Neuron outputConnection = new Neuron(mutationNode._nodeIndex, output, neuron._weight, globalInovationCounter); //Set the weight to the original weight to maintain fitness
                neuron._enabled = false; //Disable the original connection

                //Add the newly created neurons to the 'to add list'
                ToAddNeurons.Add(inputConnection);
                ToAddNeurons.Add(outputConnection);
            }

        }
       

        //Finishing up by adding the newly created connections
        foreach (Neuron neuron in ToAddNeurons)
        {
            NeuralNetwork.Add(neuron);
            newConnectionCounter++; //DEBUG
        }

        //Some information output //DEBUG
        Debug.Log("This mutation, genome#" + genomeIndex + "\nWeights changed: " + weightsChanged + "\nWeights pertured: " + weightsPertured + "\nWeights randomized: " + weightsRandomized + "\nEnabled connections: " + connectionsEnabled + "\nNew Connections: " + newConnectionCounter + "\nNew Nodes: " + newNodeCounter);
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
    /// Returns a random neuron, used for adding a node into the network
    /// </summary>
    public Neuron GetRandomNeuronConnection()
    {
        //Might have to double check that that the connection is not dissabled!
        return NeuralNetwork[(int)Random.Range(0, NeuralNetwork.Count)];
    }
    /// <summary>
    /// Returns a random node, used for adding a new connection into the network
    /// </summary>
    public Node GetRandomNode( int ignoreIndex = -1)
    {
        if (ignoreIndex != -1)
        {
            List<Node> tempList = new List<Node>();
            foreach(Node node in NodeCollection)
                if (node._type != NeuronType.OutPut && node._nodeIndex != ignoreIndex)
                    tempList.Add(node);

            return tempList[(int)Random.Range(0, tempList.Count)];
        }
        return NodeCollection[(int)Random.Range(0, NodeCollection.Count)];
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
    /// <summary>
    /// Sigmoid function, since Unity math does not have one -_-
    /// </summary>
    float Sigmoid(float x) {
        return 2 / (1 + Mathf.Exp(-4.9f * x)) - 1; //See https://www.jair.org/media/1338/live-1338-2278-jair.pdf page 95
    }
}
