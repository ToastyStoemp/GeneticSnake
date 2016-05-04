using UnityEngine;
using System.Collections;

public enum NeuronType
{
    Input,
    OutPut,
    Hidden
}

[System.Serializable]
public class Neuron {

    public int _in,
               _out,
               _inovationNum;

    public bool _enabled;
    public float _weight;


    public Neuron(int In, int Out, float weight, int Inovation)
    {
        _in = In;
        _out = Out;
        _enabled = true;
        _weight = weight;
        _inovationNum = Inovation;
    }
}

[System.Serializable]
public class Node
{
    public int _nodeIndex;
	public float _value;
	public NeuronType _type;

    public Node(int nodeIndex, NeuronType Type = NeuronType.Hidden)
    {
        _nodeIndex = nodeIndex;
		_type = Type;
    }
}

