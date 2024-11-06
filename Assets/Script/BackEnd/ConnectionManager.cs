using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Instance { get; private set; }

    public HubConnection Connection { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StartConnection();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void StartConnection()
    {
        Connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5088/gamehub") // Replace with your server URL
            .Build();

        try
        {
            await Connection.StartAsync();
            Debug.Log("Connected to SignalR server.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Could not connect to SignalR server: {ex.Message}");
        }
    }

    private void OnApplicationQuit()
    {
        Connection.StopAsync();
    }
}