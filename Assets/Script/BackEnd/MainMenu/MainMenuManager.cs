using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private SceneTransitionManager sceneTransitionManager;
    // Start is called before the first frame update
    void Start()
    {
        sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exit()
    {
        sceneTransitionManager.StartSceneTransition("SignIn");
    }
}
