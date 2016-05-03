using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum BlockType
{
    Nothing,
    Apple,
    Wall //Also Snake
}

public struct Genome
{
    public float fitness;
    public int IDNumber;
    public List<float> weights;
}

public class SnakeController : MonoBehaviour {

    public bool isAlive = true;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "point")
        {
            isAlive = false;
        }
        else
        {
            ///Add points to fitness
        }
    }
}
