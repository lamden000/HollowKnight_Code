using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrallaxEffect : MonoBehaviour
{
    public float parallaxFactor;  // Adjust this for each layer's speed
    private Vector3 lastCameraPosition;

    void Start()
    {
        lastCameraPosition = Camera.main.transform.position;
    }

    void Update()
    {
        Vector3 cameraMovement = Camera.main.transform.position - lastCameraPosition;
        transform.position += cameraMovement * parallaxFactor;
        lastCameraPosition = Camera.main.transform.position;
    }
}
