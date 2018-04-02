using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMoveScript : MonoBehaviour {

    float RoomState =0;
    public Animator anim;
   
	
	void Start ()
    {
        anim.GetComponent<Animator>();
	}	
	
	void Update ()
    {
        if (Input.GetKeyDown("1"))
        {
            MoveRoom();
        }  
        
    }

    public void MoveRoom()
    {
        if (RoomState == 0)
        {
            anim.Play("Gerüst_Rotate");
            RoomState = 1;
        }
        if (RoomState == 1)
        {
            anim.Play("Gerüst_Rotate_Back");
            RoomState = 0;
        }
    }
}
