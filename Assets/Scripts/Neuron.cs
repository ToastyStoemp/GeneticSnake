using UnityEngine;
using System.Collections;

public enum NeuronType
{
    Input,
    OutPut,
    Hidden
}

public class Neuron {

    public int _in,
               _out,
               _inovationNum;

    public bool _enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }
    public float _weight
    {
        get { return _weight; }
        set { _weight = value; }
    }


    public Neuron(int In, int Out, float weight, int Inovation)
    {
        _in = In;
        _out = Out;
        _enabled = true;
        _weight = weight;
        _inovationNum = Inovation;
    }
}

public class Node
{
    public int _nodeIndex;
    public float _value
    {
        get { return _value; }
        set { _value = value; }
    }
    public NeuronType _type
    {
        get { return _type; }
        set { _type = value; }
    }

    public Node(int nodeIndex, NeuronType Type = NeuronType.Hidden)
    {
        _nodeIndex = nodeIndex;
    }
}
}
