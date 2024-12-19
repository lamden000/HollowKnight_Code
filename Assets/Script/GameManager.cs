using System.Collections;
using System.Collections.Generic;
using Assets.Script.Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerData playerStats;
    public string LastScene;

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