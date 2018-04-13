using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour {

    public AudioSource bling;
    private bool soundState = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (soundState == null)
        {
            return;
        }

        if(soundState == true)
        {
            bling.Play();
            soundState = false;
        }

		
	}
    [PunRPC]
    public void SendSound()
    {
        soundState = true;
       
    }
}
