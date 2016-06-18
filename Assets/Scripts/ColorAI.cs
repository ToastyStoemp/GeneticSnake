using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class ColorAI : MonoBehaviour {

    private Material _mat;

    public NeatAi AI;
    public int PoolSize = 2;

    public GameObject targetColorObj;
    public Color targetColor;

    List<float> desired;

    List<float> input;

	public GameObject DebugLocation;

    public Text Generation;
    public Text Fitness;
    public Text Probability;

    float timer = 0.0f;
    float nextUpdateTime = 0.1f;

    public bool finished = false;
    public float targetFitness = 0.98f;

    

	// Use this for initialization
	void Start () {
        _mat = GetComponent<Renderer>().material;
        targetColorObj.GetComponent<Renderer>().material.color = targetColor;

        desired = new List<float>();
        desired.Add(targetColor.r); //Red
        desired.Add(targetColor.g); //Green
        desired.Add(targetColor.b); //Blue

        input = new List<float>();
        input.Add(1.0f); //Red
        input.Add(1.0f); //Green
        input.Add(1.0f); //Blue

        AI = new NeatAi();
        AI.Instantiate(desired, input, PoolSize);

        List<float> temp = AI.Tick();
        Color newColor = new Color(temp[0], temp[1], temp[2]);
        _mat.color = newColor;


    }
	
	// Update is called once per frame
	void Update () {
        if (!finished)
        {
            timer += Time.deltaTime;
            if (timer > nextUpdateTime)
            {
                NextTick();
                timer = 0.0f;
            }
        }
    }

    public void NextTick()
	{
		AI.Evolve ();
		List<float> temp = AI.Tick();
		Color newColor = new Color(temp[0], temp[1], temp[2]);
		_mat.color = newColor;

        Generation.text = AI.GetGenerationIndex().ToString();
        Fitness.text = AI.GetBestFitness()._fitness.ToString();

        if (AI.GetBestFitness()._fitness > targetFitness)
            finished = true;
	}

	void OnDrawGizmos()
	{
		AI.PrintAll(DebugLocation.transform.position);
        //AI.PrintBest(DebugLocation.transform.position);
	}

}
