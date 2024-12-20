using System.Collections;
using System.Collections.Generic;
using Assets.Script.BackEnd.User;
using Assets.Script.Player;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerData playerStats;
    public string LastScene;
    private TextMeshProUGUI coinNumText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure it persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
        if (playerStats == null)
        {
            playerStats = new PlayerData(); 
        }
    }

    private void Update()
    {
       if( Input.GetKeyDown(KeyCode.Escape))
        {
            SceneTransitionManager mn=GameObject.Find("SceneLoadingManager").GetComponent<SceneTransitionManager>();
            mn.loadLoadingScene = true;
    
            Destroy(GameObject.Find("CoinEmiter").gameObject);
            Destroy(GameObject.Find("CameraShake").gameObject);
            mn.StartSceneTransition("MainMenu");
            Destroy(gameObject);

        }
    }

    public async void SubmitPoints()
    {
        if (ConnectionManager.Instance.Connection != null &&
            ConnectionManager.Instance.Connection.State == HubConnectionState.Connected)
        {
            try
            {
                await ConnectionManager.Instance.Connection.InvokeAsync("UpdatePlayerPoints", PlayerSession.CurrentPlayer.AccountId, playerStats.Money);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to submit points: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Not connected to the server. Points not submitted.");
        }
    }

    public void UpdateMoney()
    {
        GameObject textObject = GameObject.Find("CoinNumberText");
        coinNumText = textObject.GetComponent<TextMeshProUGUI>();
        coinNumText.text = playerStats.Money.ToString();
    }

    public void GetMoney()
    {
        playerStats.Money++;
        coinNumText.text = playerStats.Money.ToString();
    }    
    
    public void ResetGame()
    {
        playerStats.Health = 0;
        playerStats.Soul = 0;
        playerStats.Money = 0;
        SceneTransitionManager stm = GameObject.Find("SceneLoadingManager").GetComponent<SceneTransitionManager>();
        stm.loadLoadingScene = true;
        stm.StartSceneTransition("Dirthmouth");
    }
}