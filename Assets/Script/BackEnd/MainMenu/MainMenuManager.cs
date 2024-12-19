using Assets.Script.BackEnd.User;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    public GameObject multiplayerPannel;
    public GameObject mainMenuPannel;
    public GameObject createRoomPanel;
    public GameObject leaderboardPanel;
    public GameObject title;
    private SceneTransitionManager sceneTransitionManager; 
    // Start is called before the first frame update
    void Start()
    {
        sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();   
    }

    public void ConnectMultiplayer()
    {      
        if(!multiplayerPannel.activeInHierarchy)
        {
            SetActiveMenu(false,multiplayerPannel);
        }
        else
        {
            SetActiveMenu(true, multiplayerPannel);
        }
    }


    void SetActiveMenu(bool isActive,GameObject activeObject)
    {
        mainMenuPannel.SetActive(isActive);
        title.SetActive(isActive);
        activeObject.SetActive(!isActive);
    }

    public void CreateRoom()
    {
        if (!createRoomPanel.activeInHierarchy)
        {
            createRoomPanel.SetActive(true);
        }
        else
        {
            createRoomPanel.SetActive(false);
        }
    }

    public void ShowLeaderboard()
    {
        if (!leaderboardPanel.activeInHierarchy)
        {
            SetActiveMenu(false, leaderboardPanel);
        }
        else
        {
            SetActiveMenu(true, leaderboardPanel);
        }
    }


    public void StartGame()
    {
        sceneTransitionManager.StartSceneTransition("Dirthmouth");
    }


    async public void Exit()
    {
        await ConnectionManager.Instance.Connection.InvokeAsync("SignOut");
        sceneTransitionManager.StartSceneTransition("SignIn");
    }
}
