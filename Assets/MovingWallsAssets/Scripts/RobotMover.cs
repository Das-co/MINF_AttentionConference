using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMover : MonoBehaviour {

    [SerializeField]
    private Animator animation;
	// Use this for initialization
	void Start () {
        animation.GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {


        
    }

    public void SendIdle() {
        GetComponent<PhotonView>().RPC("PlayIdle", PhotonTargets.All);
    }

    public void SendWalk() {
        GetComponent<PhotonView>().RPC("PlayWalk", PhotonTargets.All);
    }

    public void SendJump() {
        GetComponent<PhotonView>().RPC("PlayJump", PhotonTargets.All);
    }

    public void SendSprint() {
        GetComponent<PhotonView>().RPC("PlaySprint", PhotonTargets.All);
    }

    public void SendPunch() {
        GetComponent<PhotonView>().RPC("PlayPunch", PhotonTargets.All);
    }

    [PunRPC]
    public void PlayIdle()
    {
        //animation.Play("metarig|Robot_Idle");
        falseAll();
        animation.SetBool("Idle", true);
    }
    [PunRPC]
    public void PlayWalk()
    {
        //animation.Play("metarig|Robot_Walk");
        falseAll();
        animation.SetBool("Walk", true);
    }
    [PunRPC]
    public void PlayJump()
    {
        //animation.Play("metarig|Robot_Jump");
        falseAll();
        animation.SetBool("Jump", true);
    }
    [PunRPC]
    public void PlaySprint()
    {
        //animation.Play("metarig|Robot_Sprint");
        falseAll();
        animation.SetBool("Sprint", true);
    }
    [PunRPC]
    public void PlayPunch()
    {
        //animation.Play("metarig|Robot_Punch");
        falseAll();
        animation.SetBool("Punch", true);
    }

    private void falseAll()
    {
        animation.SetBool("Jump", false);
        animation.SetBool("Walk", false);
        animation.SetBool("Idle", false);
        animation.SetBool("Sprint", false);
        animation.SetBool("Punch", false);
    }


}
