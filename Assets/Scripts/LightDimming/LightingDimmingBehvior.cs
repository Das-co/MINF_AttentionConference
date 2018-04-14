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
    private bool ambientLight;
    private float counter;
    private float ambientIntensity;
    private Vector3 targetPos;

    // Use this for initialization
    void Start () {
        coneAttention.SetActive(false);
        reduceRiseColor = true;
        counter = RenderSettings.ambientIntensity;
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update () {
        
        transform.position = targetPos;

        ambientIntensity = RenderSettings.ambientIntensity;


        Vector3 v3Pos = Camera.main.WorldToViewportPoint(targetPos);
        
        if(ambientLight==true && reduceRiseColor == false && riseOrReduce &&RenderSettings.ambientIntensity < 1&& v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f && v3Pos.z > 0.0f)
        {
            //Debug.Log(v3Pos.x + " >= 0.0f; " + v3Pos.x + " <= 1.0f; "+ v3Pos.y +">= 0.0f; "+ v3Pos.y +"<= 1.0f; "+v3Pos.z +"> 0.0f");
            if (v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f && v3Pos.z > 0.0f)
            {
                if (ambientLight == true)
                    RenderSettings.ambientIntensity += 0.01f;
                //reduceRiseColor = false;
                print("In Cam");
                attentionLight.GetComponent<Light>().range -= 0.1f;
            }
            
        }
        else if(reduceRiseColor == false && riseOrReduce && RenderSettings.ambientIntensity > 0 && !(v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f && v3Pos.z > 0.0f))
        {
            if(ambientLight == true)
                RenderSettings.ambientIntensity -= 0.01f;
            attentionLight.GetComponent<Light>().range += 0.1f;
        }


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

    public void Sequence()
    {
        GetComponent<PhotonView>().RPC("Test", PhotonTargets.All);
        
    }

    public void ToggleLightReduce(bool boolean)
    {
        ambientLight = boolean;
    }

    [PunRPC]
    public void Test()
    {
        print("Sequence started");
        reduceRiseColor = !reduceRiseColor;
        riseOrReduce = !riseOrReduce;
        roomLights.SetActive(false);
        targetPos = GameObject.FindGameObjectWithTag("Target").transform.position;
    }
}
