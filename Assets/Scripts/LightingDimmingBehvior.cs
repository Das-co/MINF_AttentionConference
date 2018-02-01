using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingDimmingBehvior : MonoBehaviour {

    private bool reduceRiseColor;
    private bool riseOrReduce;
    private bool objectJump;
    private bool objectDir;
    public Light attentionLight;
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

            attentionLight.intensity += 1; 

            if (RenderSettings.ambientIntensity <= 0)
            {
                reduceRiseColor = false;
                objectJump = true;
                //print("Stopped reducing");
                coneAttention.SetActive(true);
            }

        }
        //Lichtstärke im Raum erhöhen
        else if (reduceRiseColor == true && !riseOrReduce)
        {
            RenderSettings.ambientIntensity += 0.01f;
            print("rising: " + RenderSettings.ambientIntensity);

            attentionLight.intensity -= 1;

            if (RenderSettings.ambientIntensity >= 1)
            {
                reduceRiseColor = false;
                objectJump = false;
                //print("Stopped rising");
                coneAttention.SetActive(false);
            }
        }

        if (objectJump == true)
        {
            
            if (coneAttention.transform.position.y >= 9.8f)
            {
                objectDir = true;
            } else if (coneAttention.transform.position.y <= 5.5f)
            {
                objectDir = false;
            }

            if (objectDir == false)
            {
                coneAttention.transform.position = new Vector3(coneAttention.transform.position.x, coneAttention.transform.position.y + 0.1f, coneAttention.transform.position.z);
                //print("Move up");
                
            }
            else 
            {
                coneAttention.transform.position = new Vector3(coneAttention.transform.position.x, coneAttention.transform.position.y - 0.1f, coneAttention.transform.position.z);
                //print("Move down");
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
