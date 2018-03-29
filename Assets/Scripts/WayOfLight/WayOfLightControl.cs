using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayOfLightControl : MonoBehaviour {

    public Transform player;
    public Transform target;
    public GameObject LightWay; 
    public GameObject theLight;
    public float speed;

    private bool destroyLights;
    private GameObject clone;
    private GameObject tempLight;
    private float playerTargetDistance;
    private float cloneTargetDistance;
    private float distanceToSpawn;
    private bool sequenceStart;
    [SerializeField]
    private bool reduceRiseColor;
    [SerializeField]
    private bool riseLight;
    private float counter;
    private bool ambientLight;
    private bool notFirstTime;

    private void Start()
    {
        ambientLight = true;
        counter = RenderSettings.ambientIntensity;
        //reduceRiseColor = true;
    }
    // Update is called once per frame
    void Update () {

        if (sequenceStart == true)
        {
            float step = speed * Time.deltaTime;
            //Debug.Log("Clone - Target distance" + Mathf.Round(Vector3.Distance(clone.transform.position, target.position)*100)/100);

            if(clone.transform.position == target.transform.position)
            {
                clone.transform.position = player.transform.position;
            }
        }

        //Lichtstärke im Raum verringern
        if (reduceRiseColor == true && riseLight)
        {
            if (ambientLight == true)
            {
                RenderSettings.ambientIntensity -= 0.01f;
                //roomLights.SetActive(false);
            }
            counter -= 0.01f;

            if (counter <= 0)
            {
                reduceRiseColor = false;
                print("Stopped reducing");
            }
        }

        //Lichtstärke im Raum erhöhen
        else if (reduceRiseColor == true && !riseLight)
        {
            if (ambientLight == true)
            {
                RenderSettings.ambientIntensity += 0.01f;
                //roomLights.SetActive(true);
            }
            counter += 0.01f;

            if (counter >= 1)
            {
                reduceRiseColor = false;
                print("Stopped rising");
            }
        }

    }

    private void ResetWay()
    {
        clone.transform.position = player.transform.position;
    }

    private void SpawnLight()
    {
        if(sequenceStart == true)
        {
            tempLight = Instantiate(theLight, new Vector3(clone.transform.position.x, 0.1f, clone.transform.position.z), Quaternion.identity);
            tempLight.tag = "SpawnedLight";
            Destroy(tempLight, 1.1f);
        }
    }

    public void Sequence()
    {
        print("Starting... ");
        sequenceStart = !sequenceStart;
        reduceRiseColor = !reduceRiseColor;
        riseLight = !riseLight;


        if (sequenceStart == false)
        {
            Destroy(clone);
        } else
        {
            destroyLights = false;
            clone = Instantiate(LightWay, new Vector3(player.transform.position.x, 0.1f, player.transform.position.z), Quaternion.identity);
            playerTargetDistance = Vector3.Distance(player.position, target.position);
            //Debug.Log("Player - Target distance: " + playerTargetDistance);

            //InvokeRepeating("ResetWay", 0.0f, 5);
            if (notFirstTime == false)
            {
                InvokeRepeating("SpawnLight", 0.2f, 0.5f);
                notFirstTime = true;
            }
        }
    }

    public void ToggleLightReduce(bool boolean)
    {
        ambientLight = boolean;
    }

}
