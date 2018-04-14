using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeadView : MonoBehaviour {
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject imageArrow;
    [SerializeField]
    private Vector3 v3Pos;
    private GameObject clone;
    private bool cntrlpanel;

    //private Camera cam; //Camera to use
    //private Transform target2; //Target to point at (you could set this to any gameObject dynamically)
    //private Vector3 targetPos; //Target position on screen
    //private Vector3 screenMiddle; //Middle of the screen
    private Vector3 dir;
    private float angle;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Target");
        imageArrow.SetActive(cntrlpanel);
    }


    void Update()
    {
        imageArrow.SetActive(cntrlpanel);
        if (cntrlpanel == true)
        {
            
            PositionArrow();
        }
    }

    public void ChangeBoolArrow()
    {
        GetComponent<PhotonView>().RPC("SetArrowBool", PhotonTargets.All);
    }

    [PunRPC]
    private void SetArrowBool()
    {
        cntrlpanel = !cntrlpanel;
    }


    void PositionArrow()
    {
        //GetComponent<Image>().enabled = false;

        v3Pos = Camera.main.WorldToViewportPoint(target.transform.position);
        //print("Target Position: " + v3Pos);

        if (v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f&&v3Pos.z > 0.0f)
        {
            //print("In Cam");
            imageArrow.GetComponent<Image>().enabled = false;
            return;
        }
        else 
        {
            imageArrow.GetComponent<Image>().enabled = true;

        }


        dir = target.transform.position - transform.position;
        angle = Mathf.Atan2(dir.x, dir.z);
        transform.eulerAngles = new Vector3(0, 0, angle);

        var targetPosLocal = Camera.main.transform.InverseTransformPoint(target.transform.position);
        var targetAngle = -Mathf.Atan2(targetPosLocal.x, targetPosLocal.y) * Mathf.Rad2Deg /*- 90*/;

        transform.eulerAngles = new Vector3(0, 0, targetAngle);

        //clone.transform.position = new Vector2(100,100);
    }
}
