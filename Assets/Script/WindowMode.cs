using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowMode : MonoBehaviour
{
    void Start()
    {
        // Load the player's preference for fullscreen
        Screen.SetResolution(800, 800, false);
        Application.runInBackground = true;
       // Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    }

}
