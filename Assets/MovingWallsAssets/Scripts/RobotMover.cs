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
    
    public void PlayIdle()
    {
        animation.Play("metarig|Robot_Idle");
    }
    public void PlayWalk()
    {
        animation.Play("metarig|Robot_Walk");
    }
    public void PlayJump()
    {
        animation.Play("metarig|Robot_Jump");
    }
    public void PlaySprint()
    {
        animation.Play("metarig|Robot_Sprint");
    }
    public void PlayPunch()
    {
        animation.Play("metarig|Robot_Punch");
    }



}
