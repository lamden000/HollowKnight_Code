using TMPro;
using UnityEngine;

public class RoomEntry : MonoBehaviour
{
    public TMP_Text RoomNameText;
    public TMP_Text PlayerCountText;
    public TMP_Text isPrivateText;

    // Method to set up the room entry with specific information
    public void SetRoomInfo(string roomName, int currentPlayerCount,bool isPrivate)
    {
        RoomNameText.text = roomName;
        PlayerCountText.text = $"{currentPlayerCount}/{4}";
        isPrivateText.text = isPrivate? "Private":"Public";
    }

    public void SetLeaderboardInfo(string roomName, int rank, int score)
    {
        RoomNameText.text = roomName;
        PlayerCountText.text = rank.ToString();
        isPrivateText.text = score.ToString();
    }
}