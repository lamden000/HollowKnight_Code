/*using UnityEngine;

namespace _Scripts
{
    public class ParallaxEffect : MonoBehaviour
    {
        private Vector2 _startingPos;
        public float AmountOfParallax;
        public Camera MainCamera;

        private void Start()
        {
            // Store the initial position of the object
            _startingPos = transform.position;
        }

        private void Update()
        {    
            ApplyParallax();
        }
        private void ApplyParallax()
        {
            // Get the current position of the camera
            if (MainCamera == null) return;

            Vector3 cameraPosition = MainCamera.transform.position;

            // Calculate the distance based on the parallax amount
            Vector2 distance = cameraPosition * AmountOfParallax;

            // Apply the new position to the object, while keeping its z-position unchanged
            transform.position = new Vector3(_startingPos.x + distance.x, _startingPos.y , transform.position.z);
        }
    }
}*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ParrallaxEffect : MonoBehaviour
{  // Adjust this for each layer's speed
    private Vector3 lastPlayerPosition;
    public Transform player;

    private TilemapRenderer tilemapRenderer;
    [Range(0, 1)]
    public float parallaxFactor;


    void Start()
    {
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        Vector3 cameraMovement = player.position - lastPlayerPosition;
        transform.position += (cameraMovement ) * parallaxFactor;
        lastPlayerPosition = player.position;
    }
}