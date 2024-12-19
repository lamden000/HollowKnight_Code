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
            // Step 1: Sort the player list by score in descending order
            var sortedList = playerList.OrderByDescending(p => p.Score).ToList();

            // Step 2: Assign ranks based on sorted list (highest score gets rank 1, etc.)
            for (int i = 0; i < sortedList.Count; i++)
            {
                sortedList[i].Rank = i + 1;  // Assign rank starting from 1
            }

            // Step 3: Clear previous entries in the UI
            foreach (Transform child in contentPanel)
            {
                Destroy(child.gameObject);  // Clear previous entries
            }

            // Step 4: Add each player to the leaderboard UI
            foreach (var player in sortedList)
            {
                GameObject newEntry = Instantiate(leaderboardEntryPrefab, contentPanel);
                RoomEntry entry = newEntry.GetComponent<RoomEntry>();
                entry.SetLeaderboardInfo(player.PlayerName, player.Rank, player.Score);
            }

            // Step 5: Find and highlight the current player's entry
            Leaderboard playerData = sortedList.Find(p => p.PlayerName == PlayerSession.CurrentPlayer.Name);

            if (playerData != null)
            {
                // Update the player's fixed UI entry
                RoomEntry playerEntryComponent = currentPlayerInfo.GetComponent<RoomEntry>();
                playerEntryComponent.SetLeaderboardInfo(playerData.PlayerName + " (you)", playerData.Rank, playerData.Score);

            }
        });
    }

}
