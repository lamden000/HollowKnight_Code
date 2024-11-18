using Assets.Script.BackEnd.MainMenu;
using Microsoft.AspNetCore.SignalR.Client;
using Assets.Script.BackEnd.User;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject leaderboardEntryPrefab;
    public Transform contentPanel;
    public GameObject currentPlayerInfo;
    private HubConnection connection;

    // Start is called before the first frame update
    void Awake()
    {
        connection = ConnectionManager.Instance.Connection;
        connection.On<List<Leaderboard>>("UpdateLeaderboard", UpdateLeaderboard);
    }

    private async void OnEnable()
    {
        await connection.InvokeAsync("SendLeaderboard");
    }

    public void UpdateLeaderboard(List<Leaderboard> playerList)
    {
        MainThreadDispatcher.Enqueue(() =>
        {

            foreach (Transform child in contentPanel)
            {
                Destroy(child.gameObject);  // Clear previous entries
            }

            foreach (var player in playerList)
            {
                GameObject newEntry = Instantiate(leaderboardEntryPrefab, contentPanel);
                RoomEntry entry = newEntry.GetComponent<RoomEntry>();
                entry.SetLeaderboardInfo(player.PlayerName, player.Rank, player.Score);
            }

            Leaderboard playerData = playerList.Find(p => p.PlayerName == PlayerSession.CurrentPlayer.Name);

            if (playerData != null)
            {
                // Update the player's fixed UI entry
                RoomEntry playerEntryComponent = currentPlayerInfo.GetComponent<RoomEntry>();
                playerEntryComponent.SetLeaderboardInfo(playerData.PlayerName+" (you)", playerData.Rank, playerData.Score);

                // Remove the player's data from the list
                playerList.Remove(playerData);
            }
        });
    }

}
