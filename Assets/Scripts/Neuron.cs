using UnityEngine;
using System.Collections;

public enum NeuronType
{
    Input,
    OutPut,
    Hidden
}

public class Neuron : MonoBehaviour {

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

    public NeuronType _type
    {
        get { return _type; }
        set { _type = value; }
    }

    public Neuron (int In, int Out, float weight, int Inovation, NeuronType Type = NeuronType.Hidden)
    {
        _in = In;
        _out = Out;
        _enabled = true;
        _weight = weight;
        _inovationNum = Inovation;
    }
}
