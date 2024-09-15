using System.Collections.Generic;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;

public class NetworkManager : MonoBehaviour
{
    private HubConnection connection;
    public GameObject playerPrefab; // Prefab của người chơi khác
    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

    async void Start()
    {
        // Kết nối tới SignalR hub
        connection = new HubConnectionBuilder()
            .WithUrl("https://hollowknightonline-dnf9g8fgfxhggrf6.eastasia-01.azurewebsites.net/gamehub")
            .Build();

        // Lắng nghe vị trí của người chơi khác
        connection.On<string, float, float>("ReceivePlayerPosition", (playerId, x, y) =>
        {
            UpdatePlayerPosition(playerId, x, y);
        });

        // Kết nối tới server
        await connection.StartAsync();
    }

    private void Update()
    {
        // Kiểm tra nếu kết nối đã sẵn sàng trước khi gửi dữ liệu
        if (connection != null && connection.State == HubConnectionState.Connected)
        {
            // Gửi vị trí của người chơi này
            SendPlayerPosition("Player1", transform.position); // Thay 'Player1' bằng ID thực tế của người chơi
        }
    }

    // Gửi vị trí của nhân vật tới server
    public async void SendPlayerPosition(string playerId, Vector2 position)
    {
        await connection.InvokeAsync("SendPlayerPosition", playerId, position.x, position.y);
    }

    // Cập nhật vị trí của người chơi khác
    private void UpdatePlayerPosition(string playerId, float x, float y)
    {
        // Kiểm tra nếu người chơi đã tồn tại
        if (!players.ContainsKey(playerId))
        {
            // Tạo đối tượng người chơi mới nếu chưa có
            GameObject playerObj = Instantiate(playerPrefab);
            playerObj.transform.position = new Vector2(x, y);
            players.Add(playerId, playerObj);
        }
        else
        {
            // Cập nhật vị trí của người chơi khác
            players[playerId].transform.position = new Vector2(x, y);
        }
    }
}