using System.Collections.Generic;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Threading.Tasks;

public class NetworkManager : MonoBehaviour
{
    private HubConnection connection;
    public GameObject playerPrefab; // Prefab của người chơi khác
    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    private Dictionary<string, Vector3> targetPositions = new Dictionary<string, Vector3>();
    public float interpolationSpeed = 5f; // Tốc độ nội suy
    private Vector3 lastPosition;
    public float positionUpdateThreshold = 0.01f; // Ngưỡng thay đổi vị trí
    public float positionUpdateInterval = 0.1f; // Khoảng thời gian giữa mỗi lần gửi
    private float positionUpdateTimer = 0f;
    async void Start()
    {
        // Kết nối tới SignalR hub
        // https://hollowknightonline-dnf9g8fgfxhggrf6.eastasia-01.azurewebsites.net/gamehub
        //https://hollowknightsever.onrender.com/gamehub
        connection = new HubConnectionBuilder()
            .WithUrl("https://hollowknightsever.onrender.com/gamehub")
            .Build();

        connection.On<string, float, float>("ReceivePlayerPosition", (playerId, x, y) =>
        {
            UpdatePlayerPosition(playerId, x, y);
        });

        connection.Closed += async (error) =>
        {
            Debug.Log("Kết nối bị ngắt, đang thử kết nối lại...");
            await Task.Delay(5000); // Đợi 5 giây trước khi thử kết nối lại
            await connection.StartAsync();
        };

        try
        {
            await connection.StartAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Kết nối thất bại: {ex.Message}");
        }

    }

    private void Update()
    {
        if (connection != null && connection.State == HubConnectionState.Connected)
        {
            // Chỉ gửi khi vị trí thay đổi đáng kể hoặc sau khoảng thời gian cố định
            positionUpdateTimer += Time.deltaTime;
            if (Vector3.Distance(lastPosition, transform.position) > positionUpdateThreshold || positionUpdateTimer >= positionUpdateInterval)
            {
                SendPlayerPosition("Player1", transform.position); // Gửi vị trí hiện tại
                lastPosition = transform.position;
                positionUpdateTimer = 0f; // Reset timer
            }

            // Duyệt qua tất cả các người chơi và cập nhật vị trí của họ
            foreach (var playerId in players.Keys)
            {
                if (targetPositions.ContainsKey(playerId))
                {
                    // Sử dụng interpolation để di chuyển đến vị trí mục tiêu
                    players[playerId].transform.position = Vector3.Lerp(players[playerId].transform.position, targetPositions[playerId], Time.deltaTime * interpolationSpeed);
                }
            }
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
        MainThreadDispatcher.Enqueue(() =>
        {
            // Đảm bảo cập nhật vị trí trên luồng chính của Unity
            if (!players.ContainsKey(playerId))
            {
                GameObject playerObj = Instantiate(playerPrefab);
                playerObj.transform.position = new Vector2(x, y);
                players.Add(playerId, playerObj);
            }

            // Lưu trữ vị trí mục tiêu
            targetPositions[playerId] = new Vector2(x, y);
        });
    }

}