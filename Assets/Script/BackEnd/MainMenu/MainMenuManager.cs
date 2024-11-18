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
            mainMenuPannel.SetActive(false);
            multiplayerPannel.SetActive(true);
        }
        else
        {
            mainMenuPannel.SetActive(true);
            multiplayerPannel.SetActive(false);
        }
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
            mainMenuPannel.SetActive(false);
            leaderboardPanel.SetActive(true);
        }
        else
        {
            mainMenuPannel.SetActive(true);
            leaderboardPanel.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
    }


    async public void Exit()
    {
        await ConnectionManager.Instance.Connection.InvokeAsync("SignOut");
        sceneTransitionManager.StartSceneTransition("SignIn");
    }
}
