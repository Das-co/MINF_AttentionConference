using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour {

    public AudioSource bling;
    private bool soundState = false;
    bool soundBool;


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BlingSound(bool boolean)
    {
        soundBool = boolean;
        GetComponent<PhotonView>().RPC("SendSound", PhotonTargets.All);
    }

    [PunRPC]
    public void SendSound()
    {
        soundState = soundBool;
        bling.Play();
        soundState = false;
    }
}
