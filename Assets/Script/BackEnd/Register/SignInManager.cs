using Assets.Script.BackEnd.User;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Identity.Client;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignInManager : MonoBehaviour
{
    public TMP_InputField usernameInput; // Input field for username
    public TMP_InputField passwordInput; // Input field for password
    public Button signInButton; // Button to trigger sign-in
    public TMP_Text resultText; // Text field for displaying results
    private SceneTransitionManager sceneTransitionManager;

    enum AuthenticationResult { Success = 0, Incorrect = -1, IsActive = -2 }

    private void Start()
    {
        sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
        // Subscribe to the authentication result event from the connection
        ConnectionManager.Instance.Connection.On<int>("AuthenticationResult", (playerId) =>
        {

            MainThreadDispatcher.Enqueue(() =>
            {
                if (playerId==(int)AuthenticationResult.Incorrect)
                {
                    resultText.color = Color.red;

                    resultText.text = "Invalid username or password.";
                }
                else if(playerId == (int)AuthenticationResult.IsActive)
                {
                    resultText.color = Color.red;
                    resultText.text = "The account is already in use";
                }
                else
                {
                    resultText.color = Color.blue;

                    PlayerSession.SignIn(playerId, usernameInput.text);
                    resultText.text = "Sign in successful!";
                    sceneTransitionManager.StartSceneTransition("MainMenu");
                }
            });
        });
    }

    public async void OnSignInButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        // Ensure the connection is established before sending the request
        if (ConnectionManager.Instance.Connection.State == HubConnectionState.Disconnected)
        {
            await ConnectionManager.Instance.Connection.StartAsync();
        }

        // Send authentication request to the server
        await ConnectionManager.Instance.Connection.InvokeAsync("Authenticate", username, password);
    }
}
