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
        //animation.Play("metarig|Robot_Idle");
        falseAll();
        animation.SetBool("Idle", true);
    }
    public void PlayWalk()
    {
        //animation.Play("metarig|Robot_Walk");
        falseAll();
        animation.SetBool("Walk", true);
    }
    public void PlayJump()
    {
        //animation.Play("metarig|Robot_Jump");
        falseAll();
        animation.SetBool("Jump", true);
    }
    public void PlaySprint()
    {
        //animation.Play("metarig|Robot_Sprint");
        falseAll();
        animation.SetBool("Sprint", true);
    }
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
