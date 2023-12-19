using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketship : MonoBehaviour
{
    [SerializeField] float mainThrust = 2000f;
    [SerializeField] float rotationThrust = 500f;

    Rigidbody myRigidBody;// gravity effect
    AudioSource myAudioSource; // Audio Source
    GameController gameController;
    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
        gameController = FindAnyObjectByType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {

            RocketMovement();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {   if(!isAlive || !gameController.collisionEnabled)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("I am Okay");
                break;
            case "Finish":
                myRigidBody.isKinematic = true;
                gameController.NextLevel(); 
                break;

            default:
                isAlive = false;
                gameController.ResetGame();
                break;
        }

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
        myRigidBody.freezeRotation = true;// To fix our rotation

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed); // we now have the time between each frame and no matter the flucutation the time frame will be same  
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        myRigidBody.freezeRotation = false;
    }
    private void LateUpdate()// after completing the update we want to reset our rocket rotation
    {
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);// this helps in moving our rocket haywire.
    }
}
