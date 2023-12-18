using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketship : MonoBehaviour
{
    float mainThrust = 2000f;
    float rotationThrust = 500f;

    Rigidbody myRigidBody;// gravity effect
    AudioSource myAudioSource; // Audio Source

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();    
    }

    // Update is called once per frame
    void Update()
    {
        RocketMovement();
    }

    private void RocketMovement()
    {
        float rotationSpeed = Time.deltaTime * rotationThrust;
        Thrusting();
        Rotating(rotationSpeed);
    }

    private void Thrusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!myAudioSource.isPlaying) // For Sound Overlapping Condition
            {
                myAudioSource.Play();
            }
            myRigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime); // it adds the force relative to the way our coordinate system is facing.
        }
        else
        {
            myAudioSource.Stop(); // Stop if we dont press the space button
        }
    }

    private void Rotating(float rotationSpeed)
    {
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
