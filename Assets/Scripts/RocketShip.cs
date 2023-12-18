using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RocketMovement();
    }

    private static void RocketMovement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            print("Flying");
        }
        if (Input.GetKey(KeyCode.A))
        {
            print("Turning left");
        }
        if (Input.GetKey(KeyCode.D))
        {
            print("Turning right");
        }
    }
}
