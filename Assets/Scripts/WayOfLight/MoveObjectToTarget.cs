using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectToTarget : MonoBehaviour {

    private Transform target;
    public float speed;
    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Target").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed);

        //if (transform.position == target.position)
        //{
        //    Destroy(gameObject);
        //    Debug.Log("Destroyed");
        //}
    }
}
