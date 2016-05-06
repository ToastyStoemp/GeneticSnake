using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ColorAI : MonoBehaviour {

    private Material _mat;

    public NeatAi AI;

    public GameObject targetColorObj;
    public Color targetColor;

    List<float> desired;

    List<float> input;

	public GameObject DebugLocation;

    float timer = 0.0f;
    float nextUpdateTime = 2.0f;

    

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
        AI.Instantiate(desired, input, 1);

        List<float> temp = AI.Tick();
        Color newColor = new Color(temp[0], temp[1], temp[2]);
        _mat.color = newColor;


    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > nextUpdateTime)
        {
		NextTick ();
            timer = 0.0f;
        }
	}

	public void NextTick()
	{
		AI.Mutate ();
		List<float> temp = AI.Tick();
		Color newColor = new Color(temp[0], temp[1], temp[2]);
		_mat.color = newColor;
	}

	void OnDrawGizmos()
	{
		AI.Print (0, 0, DebugLocation.transform.position);
	}

}
