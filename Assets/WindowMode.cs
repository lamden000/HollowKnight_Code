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
    }

    public void ToggleFullscreen()
    {
        // Toggle fullscreen mode
        bool isFullScreen = !Screen.fullScreen;
        Screen.fullScreen = isFullScreen;

        // Save the player's preference
       // PlayerPrefs.SetInt("fullscreen", isFullScreen ? 1 : 0);
       // PlayerPrefs.Save();
    }
}
