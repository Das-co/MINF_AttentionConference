using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBehvior : MonoBehaviour {

    private bool reduceRiseColor;
    private bool riseOrReduce;
    public Light[] attentionLight;
    public GameObject coneAttention;
    public GameObject roomLights;


    // Use this for initialization
    void Start () {
        coneAttention.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.L) ){
            
            print("L key was pressed" + reduceRiseColor);
            reduceRiseColor=true;
            riseOrReduce = !riseOrReduce;
            roomLights.SetActive(!riseOrReduce);
        }

        //Lichtstärke im Raum verringern
        if (reduceRiseColor == true && riseOrReduce)
        {
            RenderSettings.ambientIntensity -= 0.01f;
            print("reducing: " + RenderSettings.ambientIntensity);
            for(int i=0; i<attentionLight.Length; i++)
            {
            attentionLight[i].intensity += 1; 

            }

            if (RenderSettings.ambientIntensity <= 0)
            {
                reduceRiseColor = false;
                //print("Stopped reducing");
                coneAttention.SetActive(true);
            }

        }
        //Lichtstärke im Raum erhöhen
        else if (reduceRiseColor == true && !riseOrReduce)
        {
            RenderSettings.ambientIntensity += 0.01f;
            print("rising: " + RenderSettings.ambientIntensity);

            for (int i = 0; i < attentionLight.Length; i++)
            {
                attentionLight[i].intensity -= 1;

            }

            if (RenderSettings.ambientIntensity >= 1)
            {
                reduceRiseColor = false;
                //print("Stopped rising");
                coneAttention.SetActive(false);
            }
        }

        


    }

    IEnumerator Example()
    {
        print(Time.time);
        yield return new WaitForSeconds(1);
        print(Time.time);
    }
}
