﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBehvior : MonoBehaviour {
    [SerializeField]
    private bool reduceRiseColor;
    [SerializeField]
    private bool riseOrReduce;
    [SerializeField]
    private bool ambientLight;
    private float counter;
    public Light[] attentionLight;
    public GameObject coneAttention;
    public GameObject roomLights;
    public ControlPanel cPanel;


    // Use this for initialization
    void Start () {
        coneAttention.SetActive(false);
        reduceRiseColor = true;
        counter = RenderSettings.ambientIntensity;
    }

    void Update () {


        //Lichtstärke im Raum verringern
        if (reduceRiseColor == true && riseOrReduce)
        {
            coneAttention.SetActive(true);

            if (ambientLight == true)
            {
                RenderSettings.ambientIntensity -= 0.01f;
                roomLights.SetActive(false);
            }
            counter -= 0.01f;
            //print("reducing: " + RenderSettings.ambientIntensity);
            for(int i=0; i<attentionLight.Length; i++)
            {
                attentionLight[i].intensity += 2; 
            }

            if (counter <= 0)
            {
                reduceRiseColor = false;
                //print("Stopped reducing");
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

            //print("rising: " + RenderSettings.ambientIntensity);

            for (int i = 0; i < attentionLight.Length; i++)
            {
                attentionLight[i].intensity -= 2;

            }

            if (counter >= 1)
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

    public void Sequence()
    {
        print("Starting... " + reduceRiseColor);
        reduceRiseColor = !reduceRiseColor;
        riseOrReduce = !riseOrReduce;
    }

    public void ToggleLightReduce(bool boolean)
    {
        ambientLight = boolean;
    }
}