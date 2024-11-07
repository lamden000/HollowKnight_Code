using Assets.Script.BackEnd;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Linq;
using Microsoft.AspNetCore.SignalR.Client;

public class RoomManager : MonoBehaviour
{
    public GameObject roomEntryPrefab;
    public TMP_InputField password;
    public Toggle isPrivateToggle;
    public Transform contentPanel;
    private Dictionary<string, Room> currentRoomList;
    private HubConnection connection;

    // Start is called before the first frame update
    async void Awake()
    {
        password.enabled = false;
        connection = ConnectionManager.Instance.Connection;
        connection.On<Dictionary<int, Room>>("UpdateRoomList", UpdateRoomList);
    }

    private async void OnEnable()
    {
        await connection.InvokeAsync("GetRoomList");
    }

    public async void CreateRoom()
    {
        // Ensure the connection is established before sending the request
        if (ConnectionManager.Instance.Connection.State == HubConnectionState.Disconnected)
        {
            await connection.StartAsync();
        }

        // Send authentication request to the server
        await connection.InvokeAsync("CreateRoom", isPrivateToggle.isOn, password.text);
    }

    public void UpdateRoomList(Dictionary<int, Room> roomList)
    {
        MainThreadDispatcher.Enqueue(() =>
        {

            foreach (Transform child in contentPanel)
            {
                Destroy(child.gameObject);  // Clear previous entries
            }

            foreach (var room in roomList.Values)
            {
                GameObject newEntry = Instantiate(roomEntryPrefab, contentPanel);
                RoomEntry entry = newEntry.GetComponent<RoomEntry>();
                entry.SetRoomInfo(room.HostName, room.CurrentPlayerCount, room.IsPrivate);

                // Add listener for joining the room
                int roomId = room.RoomId;
            //    newEntry.GetComponent<Button>().onClick.AddListener(() => OnRoomSelected(roomId, room.IsPrivate));
            }
        });
    }

    public void EnablePasswordField()
    {
        if (!isPrivateToggle.isOn)
        {
            password.enabled = false;
            password.text = "Set the room to private to enable password";
        }
        else
        {
            password.enabled = true;
            password.text = "Enter your password...";
        }
    } 

    public void OnRoomSelected(int roomId, bool isPrivate)
    {

    }

    public void FilterRooms(string searchQuery)
    {
        var filteredRooms = currentRoomList.Values.Where(room => room.HostName.Contains(searchQuery,StringComparison.OrdinalIgnoreCase));
        UpdateRoomList(filteredRooms.ToDictionary(r => r.RoomId));
    }
}
