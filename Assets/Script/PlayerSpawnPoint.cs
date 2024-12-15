using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    public enum CameraType
    {
        FlowPlayerCam=0,
        NoYFollowCam=1,
        NoFollowCam=2
    }

    public CameraType cameraType;
}
