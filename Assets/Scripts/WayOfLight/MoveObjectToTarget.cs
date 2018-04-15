using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectToTarget : MonoBehaviour {
    [SerializeField]
    private Vector3 target;
    public float speed;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed);

    }

    public void SetTarget(Vector3 theTarget)
    {
        target = theTarget;
    }
}
