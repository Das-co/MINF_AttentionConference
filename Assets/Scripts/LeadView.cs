using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeadView : MonoBehaviour {
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject arrow;
    [SerializeField]
    private Vector3 v3Pos;
    [SerializeField]
    private bool cntrlpanel;
    [SerializeField]
    private bool arrowBool;
    private bool instantiated;

    //private Camera cam; //Camera to use
    //private Transform target2; //Target to point at (you could set this to any gameObject dynamically)
    //private Vector3 targetPos; //Target position on screen
    //private Vector3 screenMiddle; //Middle of the screen
    private Vector3 dir;
    private float angle;

    private void Start()
    {
        arrow.SetActive(cntrlpanel);
    }


    void Update()
    {
        if(target==null)
            target = GameObject.FindGameObjectWithTag("Target");

        arrow.SetActive(cntrlpanel);

        if (cntrlpanel == true)
        {
            PositionArrow();
        }

        
    }

    public void ChangeBoolArrow(bool boolean)
    {
        arrowBool = boolean;
    }

    public void InstantiateArrow()
    {
        instantiated = !instantiated;
        if(instantiated==true && arrowBool==true)
            GetComponent<PhotonView>().RPC("SetArrowBoolTrue", PhotonTargets.All);
        else
            GetComponent<PhotonView>().RPC("SetArrowBoolFalse", PhotonTargets.All);
    }

    [PunRPC]
    public void SetArrowBoolFalse()
    {
        cntrlpanel = false;
    }
    [PunRPC]
    public void SetArrowBoolTrue()
    {
        cntrlpanel = true;
    }



    void PositionArrow()
    {
        //GetComponent<Image>().enabled = false;

        v3Pos = Camera.main.WorldToViewportPoint(target.transform.position);
        //print("Target Position: " + v3Pos);

        if (v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f&&v3Pos.z > 0.0f)
        {
            arrow.SetActive(false);
            //print("In Cam");
            //arrow.GetComponent<Image>().enabled = false;
            return;
            
        }
        else 
        {
            //arrow.GetComponent<Image>().enabled = true;
            arrow.SetActive(true);
        }

        //When UI-Overlay
        //dir = target.transform.position - transform.position;
        //angle = Mathf.Atan2(dir.x, dir.z);
        //transform.eulerAngles = new Vector3(0, 0, angle);

        //var targetPosLocal = Camera.main.transform.InverseTransformPoint(target.transform.position);
        //var targetAngle = -Mathf.Atan2(targetPosLocal.x, targetPosLocal.y) * Mathf.Rad2Deg /*- 90*/;

        //transform.eulerAngles = new Vector3(0, 0, targetAngle);
        //transform.eulerAngles = new Vector3(0, 0, 0);

        //Worldspace
        //arrow.transform.eulerAngles = new Vector3(arrow.transform.position.x, arrow.transform.position.y + 90, arrow.transform.position.z);
        Vector3 targetDir = target.transform.position - arrow.transform.position;
        /*arrow.transform.position*/
        Vector3 newDir = Vector3.RotateTowards(arrow.transform.forward, targetDir, 20 * Time.deltaTime, 5.0f);
        Debug.DrawRay(arrow.transform.position, newDir, Color.red);
        //print(newDir);
        arrow.transform.rotation = Quaternion.LookRotation(newDir);
        //transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
