using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerController : MonoBehaviour
{
    public float force;
    public Rigidbody rd;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            rd.angularVelocity = Vector3.zero;
            rd.velocity = Vector3.zero;
        }
        if (Input.GetKey(KeyCode.W))
        {
            rd.AddForceAtPosition(new Vector3(0, force, 0), new Vector3(0, 0, 1.5f));
        }
        if (Input.GetKey(KeyCode.A))
        {
            rd.AddForceAtPosition(new Vector3(0, force, 0), new Vector3(1.5f, 0, 0));
        }

        if (Input.GetKey(KeyCode.S))
        {
            rd.AddForceAtPosition(new Vector3(0, force, 0), new Vector3(0, 0, -1.5f));
        }
        if (Input.GetKey(KeyCode.D))
        {
            rd.AddForceAtPosition(new Vector3(0, force, 0), new Vector3(-1.5f, 0, 0));
        }
    }
}
