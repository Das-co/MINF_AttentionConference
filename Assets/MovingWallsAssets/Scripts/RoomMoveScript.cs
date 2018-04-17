using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMoveScript : MonoBehaviour {

    float RoomState =0;
    public Animator anim;
    bool roomBool;
    bool moveBool;
   
	
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

    public void SendRoom(/*bool boolean*/) {
        //roomBool = boolean;
        GetComponent<PhotonView>().RPC("MoveRoom", PhotonTargets.All);
    }


    [PunRPC]
    public void MoveRoom()
    {
        //moveBool = roomBool; 
        if (RoomState == 0)
        {
            anim.Play("Gerüst_Rotate");
            //anim.SetBool("Rotate", true);
            RoomState = 1;
        }
        if (RoomState == 1)
        {
            anim.Play("Gerüst_Rotate_Back");
            RoomState = 0;
        }
    }
}
