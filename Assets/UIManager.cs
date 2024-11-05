using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject signInPanel;
    public GameObject signUpPanel;

    public void ShowSignIn()
    {
        signInPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }

    public void ShowSignUp()
    {
        signInPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }
}
