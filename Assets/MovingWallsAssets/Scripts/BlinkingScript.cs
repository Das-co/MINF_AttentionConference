using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlinkingScript : MonoBehaviour {

    public Material lightOff;
    public Material lightOn;
    public float offSetTime = 1; //Blink speed
    float LightState = 0;
    public List<Transform> lightObjects;
    public float WaitTime = 3;

	// Use this for initialization
	void Start () {
        lightObjects = new List<Transform>();
        foreach(Transform child in transform)
        {
            lightObjects.Add(child);
        }
        lightObjects.OrderBy(go=>go.name);
       
	}
    IEnumerator Blink()
    {
        while (true)
        {
            for(int i =0; i < lightObjects.Count; i++)
            {
                lightObjects[i].GetComponent<Renderer>().material = lightOn;
                lightObjects[i].GetChild(0).GetComponent<Light>().enabled = true;
                yield return new WaitForSeconds(offSetTime);
                lightObjects[i].GetComponent<Renderer>().material = lightOff;
                lightObjects[i].GetChild(0).GetComponent<Light>().enabled = false;
            }

        }
    }

  /*  IEnumerator NoBlink()
    {
        while (true)
        {
            for (int i = 0; i < lightObjects.Count; i++)
            {
                lightObjects[i].GetComponent<Renderer>().material = lightOff;
                lightObjects[i].GetChild(0).GetComponent<Light>().enabled = false;
            }
        }
    }
    */
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown("2"))
        {
            StartCoroutine(Blink());
           
        }
        if (Input.GetKeyDown("3"))
        {
            StopAllCoroutines();
        }
    }
}
