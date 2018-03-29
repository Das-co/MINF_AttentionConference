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
    [SerializeField]
    private bool ambientLight;
    [SerializeField]
    private float counter;

    // Use this for initialization
    void Start () {
        coneAttention.SetActive(false);
        reduceRiseColor = true;
        counter = RenderSettings.ambientIntensity;
    }
	
	// Update is called once per frame
	void Update () {

        //Lichtstärke im Raum verringern
        if (reduceRiseColor == true && riseOrReduce)
        {
            if (ambientLight == true)
            {
                RenderSettings.ambientIntensity -= 0.01f;
                roomLights.SetActive(false);
            }
            counter -= 0.01f;

            attentionLight.intensity += 1; 

            if (counter <= 0)
            {
                reduceRiseColor = false;
                //print("Stopped reducing");
                coneAttention.SetActive(true);
            }
        }

        //Lichtstärke im Raum erhöhen
        else if (reduceRiseColor == true && !riseOrReduce)
        {
            if (ambientLight == true)
            {
                RenderSettings.ambientIntensity += 0.01f;
                roomLights.SetActive(true);
            }
            counter += 0.01f;

            attentionLight.intensity -= 1;

            if (counter >= 1)
            {
                reduceRiseColor = false;
                objectJump = false;
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

    public void Sequence()
    {
        print("Sequence started");
        reduceRiseColor = !reduceRiseColor;
        riseOrReduce = !riseOrReduce;
        roomLights.SetActive(false);
    }

    public void ToggleLightReduce(bool boolean)
    {
        ambientLight = boolean;
    }
}
