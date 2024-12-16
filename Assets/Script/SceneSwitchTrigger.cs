using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchTrigger : MonoBehaviour
{
    private SceneTransitionManager sceneTransitionManager;
    public string targetScene;
    void Start()
    {
        sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
             sceneTransitionManager.StartSceneTransition(targetScene);
    }
}
