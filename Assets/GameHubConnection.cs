using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHubConnection : MonoBehaviour
{
    private HubConnection connection;
    public TMP_InputField usernameInput; // For TextMeshPro input
    public TMP_InputField passwordInput; // For TextMeshPro input
    public Button signInButton; // Standard button
    public TextMeshProUGUI statusText; // For TextMeshPro text

    private void Start()
    {
        // Initialize SignalR connection
        connection = new HubConnectionBuilder()
            .WithUrl("https://vominhkiet-dvhcbjapa0hbgvea.eastasia-01.azurewebsites.net/gamehub") 
            .Build();

        // Setup event listeners for the SignalR client
        connection.On<bool>("AuthenticationResult", (isAuthenticated) =>
        {
            if (isAuthenticated)
            {
                statusText.text = "Login successful!";
                // Enable game features or move to next scene
            }
            else
            {
                statusText.text = "Login failed. Please try again.";
            }
        });

        // Start the connection
        ConnectToHub();

        // Assign SignIn method to the button click event
        signInButton.onClick.AddListener(SignIn);
    }

    // Connect to SignalR hub
    private async void ConnectToHub()
    {
        try
        {
            await connection.StartAsync();
            statusText.text = "Connected to SignalR hub!";
        }
        catch (System.Exception ex)
        {
            statusText.text = $"Connection failed: {ex.Message}";
        }
    }

    // SignIn method that gets called when the button is clicked
    public async void SignIn()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            statusText.text = "Please enter both username and password.";
            return;
        }

        try
        {
            // Invoke Authenticate method on the server
            await connection.InvokeAsync("Authenticate", username, password);
        }
        catch (System.Exception ex)
        {
            statusText.text = $"Sign in failed: {ex.Message}";
        }
    }

    private async void OnApplicationQuit()
    {
        if (connection != null)
        {
            await connection.StopAsync();
            await connection.DisposeAsync();
        }
    }
}
