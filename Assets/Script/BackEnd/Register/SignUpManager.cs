using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_InputField emailInput;
    public TMP_Text feedbackText;

    private HubConnection connection;

    private void Start()
    {
        // Set up SignalR connection
        connection = ConnectionManager.Instance.Connection;

        // Handle server response for registration result
        connection.On<bool, string>("RegisterResult", OnRegisterResult);
    }

    public void OnSignUpButtonPressed()
    {
        string username = usernameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;
        string confirmPassword=confirmPasswordInput.text;

        // Validate inputs
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)||string.IsNullOrEmpty(confirmPassword))
        {
            feedbackText.text = "All fields are required.";
            return;
        }
 
        if(!password.Equals(confirmPassword))
        {
            feedbackText.text = "Both passwords did not match";
            return;
        }

        feedbackText.text = "";

        // Send the registration request to the server
        connection.InvokeAsync("RegisterAccount", username, password, email);
    }

    private IEnumerator ShowSignIn()
    {
        usernameInput.text = "";
        passwordInput.text = "";
        emailInput.text = "";
        confirmPasswordInput.text = "";
        feedbackText.color = Color.blue;
        feedbackText.text = "Account created successfully!, you will be redirected to sign in window";
        yield return new WaitForSeconds(3);
        
       RegisterUIManager manager = GameObject.Find("UIManager").GetComponent<RegisterUIManager>();
       manager.ShowSignIn();
    }

    private void OnRegisterResult(bool success, string message)
    {
        MainThreadDispatcher.Enqueue(() =>
        {
            if (success)
            {
                StartCoroutine(ShowSignIn());
            }
            else
            {
                feedbackText.text = message;
                Debug.Log(message);
            }
        });
    }
}
