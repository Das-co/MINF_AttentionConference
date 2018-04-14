using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour {

    public AudioSource bling;
    private bool soundState = false;
    bool soundBool;
    public int times = 4;
    float delay = 1.4f;

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
        StartCoroutine(PlayAudio());
        soundState = false;
    }

    IEnumerator PlayAudio()
    {
        for (int i =0; i<times; i++)
        {
            bling.Play();
            yield return new WaitForSeconds(delay);
        }
    }
}
