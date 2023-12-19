using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketship : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] float mainThrust = 2000f;
    [SerializeField] float rotationThrust = 500f;
    [SerializeField] AudioClip mainEngine, deathExplosionSFX, successLevelSFX;
    [SerializeField] ParticleSystem mainEngineParticles,explosionParticles;

    Rigidbody myRigidBody;// gravity effect
    AudioSource myAudioSource; // Audio Source
    GameController gameController;
    HealthBar myHealthBar;


    bool isAlive = true;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
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
                SuccessRoutine();
                break;

            default:
                TakeDamage(20);
                break;
        }

    }

    private void DeathRoutine()
    {
        isAlive = false;
        explosionParticles.Play();
        AudioSource.PlayClipAtPoint(deathExplosionSFX, Camera.main.transform.position);
        gameController.ResetGame();
    }

    private void SuccessRoutine()
    {
        myRigidBody.isKinematic = true;
        AudioSource.PlayClipAtPoint(successLevelSFX, Camera.main.transform.position);
        gameController.NextLevel();
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        myHealthBar.SetHealth(currentHealth);

        if(currentHealth == 0)
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
