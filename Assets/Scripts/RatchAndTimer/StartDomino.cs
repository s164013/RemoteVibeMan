using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDomino : MonoBehaviour
{

    private Vector3 force;

    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        force = new Vector3(5, 0, 0);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(force, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
