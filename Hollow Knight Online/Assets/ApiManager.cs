using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    public string apiUrl = "https://hollowknightonline-dnf9g8fgfxhggrf6.eastasia-01.azurewebsites.net/api/game/player-movement";

    public void SendPlayerPosition(string playerId, Vector2 position)
    {
        StartCoroutine(SendPlayerPositionCoroutine(playerId, position));
    }

    private IEnumerator SendPlayerPositionCoroutine(string playerId, Vector2 position)
    {
        PlayerData playerData = new PlayerData
        {
            PlayerId = playerId,
            X = position.x,
            Y = position.y
        };

        string jsonData = JsonUtility.ToJson(playerData);
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Position sent successfully.");
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string PlayerId;
        public float X;
        public float Y;
    }
}
