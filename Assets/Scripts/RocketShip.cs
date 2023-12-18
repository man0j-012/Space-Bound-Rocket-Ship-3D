using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketship : MonoBehaviour
{
    float mainThrust = 2000f;
    float rotationThrust = 500f;

    Rigidbody myRigidBody;// gravity effect
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RocketMovement();
    }

    private void RocketMovement()
    {
        float rotationSpeed = Time.deltaTime * rotationThrust;

        if (Input.GetKey(KeyCode.Space))
        {
            myRigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime); // it adds the force relative to the way our coordinate system is facing.
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed); // we now have the time between each frame and no matter the flucutation the time frame will be same  
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
    }
}
