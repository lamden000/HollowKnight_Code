using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject signInPanel;
    public GameObject signUpPanel;
    public TMP_Text feedBackText;

    public void ShowSignIn()
    {
        signInPanel.SetActive(true);
        signUpPanel.SetActive(false);
        feedBackText.text = "";
    }

    public void ShowSignUp()
    {
        signInPanel.SetActive(false);
        signUpPanel.SetActive(true);
        feedBackText.text = "";
    }
}
