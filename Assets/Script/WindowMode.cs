using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowMode : MonoBehaviour
{
    void Start()
    {
        // Load the player's preference for fullscreen
        Screen.SetResolution(1920, 1080, true);
        Application.runInBackground = true;
       // Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    }

}
