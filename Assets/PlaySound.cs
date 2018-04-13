using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    [SerializeField]
    AudioSource audioSour;
    [SerializeField]
    AudioClip theSound;
    PhotonView photonView;

    // Use this for initialization
    void Start () {
        photonView = GetComponent<PhotonView>();
        //audioSour = GetComponent<AudioSource> ();
        audioSour.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        photonView.RPC("BingSound", PhotonTargets.All);
    }

    [PunRPC]
    public void BingSound()
    {
        AudioSource audioRPC = gameObject.AddComponent<AudioSource>();
        audioRPC.clip = theSound;
        audioRPC.spatialBlend = 1;
        audioRPC.minDistance = 25;
        audioRPC.maxDistance = 100;
        audioRPC.Play();

    }
}
