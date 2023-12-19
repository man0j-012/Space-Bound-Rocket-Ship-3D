using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] float speed = 4f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed* Time.deltaTime, 0); // Rotate the camera around the Y-axis based on the specified speed.
    }
}
