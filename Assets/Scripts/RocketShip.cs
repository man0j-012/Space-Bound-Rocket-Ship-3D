using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketship : MonoBehaviour
{
    // Serialized fields allow private variables to be edited in the Unity inspector.

    [SerializeField] int maxHealth = 100;
    [SerializeField] float mainThrust = 2000f;
    [SerializeField] float rotationThrust = 500f;
    [SerializeField] AudioClip mainEngine, deathExplosionSFX, successLevelSFX;
    [SerializeField] ParticleSystem mainEngineParticles,explosionParticles;

    // Components and variables for the rocket's functionality
    Rigidbody myRigidBody;// gravity effect
    AudioSource myAudioSource; // Audio Source
    GameController gameController;
    HealthBar myHealthBar;


    bool isAlive = true;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        // Getting necessary components and setting initial values.
        myRigidBody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();

        gameController = FindObjectOfType<GameController>();
        myHealthBar = FindObjectOfType<HealthBar>();

        currentHealth = maxHealth;
        myHealthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive) // Check if the rocket is alive, then handle movement.
        {

            RocketMovement();
        }
    }

    private void OnCollisionEnter(Collision collision)  // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
    {   if(!isAlive || !gameController.collisionEnabled) // Check if the rocket is not alive or if collisions are disabled, then return.
        {
            return;
        }
        // Check the tag of the object collided with and perform actions accordingly.
        switch (collision.gameObject.tag)  
        {
            case "Friendly":
                print("I am Okay");
                break;
            case "Finish":
                SuccessRoutine();
                break;

            default:
                TakeDamage(20);
                break;
        }

    }

    private void DeathRoutine()     // Handle actions when the rocket dies.

    {
        isAlive = false;
        explosionParticles.Play();
        AudioSource.PlayClipAtPoint(deathExplosionSFX, Camera.main.transform.position);

        FindObjectOfType<ShakeCam>().ShakeCamera();         // Trigger camera shake and reset the game through the GameController.
        gameController.ResetGame();
    }

    private void SuccessRoutine()     // Handle actions when the rocket successfully completes a level.
    {
        myRigidBody.isKinematic = true;
        AudioSource.PlayClipAtPoint(successLevelSFX, Camera.main.transform.position);
        gameController.NextLevel();
    }

    void TakeDamage(int damage)     // Handle taking damage and update health bar.
    {
        currentHealth -= damage;

        myHealthBar.SetHealth(currentHealth);         // Check if health has reached zero, then trigger DeathRoutine.

        if (currentHealth == 0)
        {
            DeathRoutine();
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
                myAudioSource.PlayOneShot(mainEngine);
            }
            mainEngineParticles.Play();
            myRigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime); // it adds the force relative to the way our coordinate system is facing.
        }
        else
        {
            mainEngineParticles.Stop();
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
