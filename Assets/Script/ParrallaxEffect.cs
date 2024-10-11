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
        tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    void Update()
    {
        Vector3 cameraMovement = player.position - lastPlayerPosition;
        transform.position += (cameraMovement-new Vector3(0,cameraMovement.y,0)) * parallaxFactor;
        lastPlayerPosition = player.position;
       
    }

}
