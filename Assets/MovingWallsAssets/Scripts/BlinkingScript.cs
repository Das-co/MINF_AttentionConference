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

    bool lightBool;
    bool lightBool2;
    bool lightsOn;
    bool lightsOff;

	

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
            for(int i = 0; i < lightObjects.Count; i++)
            {
                lightObjects[i].GetComponent<Renderer>().material = lightOn;
                lightObjects[i].GetChild(0).GetComponent<Light>().enabled = true;
                yield return new WaitForSeconds(offSetTime);
                lightObjects[i].GetComponent<Renderer>().material = lightOff;
                lightObjects[i].GetChild(0).GetComponent<Light>().enabled = false;
            }

        }
    }


    public void SendLight(bool boolean)
    {
        lightBool = boolean;
        GetComponent<PhotonView>().RPC("StartLights", PhotonTargets.All);
    }

    public void SendNoLight(bool boolean2)
    {
        lightBool2 = boolean2;
        GetComponent<PhotonView>().RPC("StopLights", PhotonTargets.All);
    }

    [PunRPC]
    public void StartLights()
    {
        lightsOn = lightBool;
        StartCoroutine(Blink());
    }

    [PunRPC]
    public void StopLights()
    {
        lightsOff = lightBool2;
        StopAllCoroutines();

        for (int i = 0; i < lightObjects.Count; i++)
        {
            lightObjects[i].GetComponent<Renderer>().material = lightOff;
            lightObjects[i].GetChild(0).GetComponent<Light>().enabled = false;
        }
    }
}
