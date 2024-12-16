using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject playerPrefab;

    [SerializeField]
    private CinemachineVirtualCamera[] _allVirtualCameras;

    private void Start()
    {
        string lastScene = GameManager.Instance.LastScene;
        // Find the corresponding spawn point
        Transform spawnPoint = GameObject.Find($"SpawnPoint_{lastScene}")?.transform;

        if (spawnPoint != null)
        {
            // Move existing player or instantiate at spawn point
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                player.transform.position = spawnPoint.position;
                player.transform.rotation = spawnPoint.rotation;
            }
            
            for(int i=0; i < _allVirtualCameras.Length; i++)
            {
                if((int)spawnPoint.GetComponent<PlayerSpawnPoint>().cameraType != i)
                {
                    _allVirtualCameras[i].GetComponent<CinemachineVirtualCamera>().enabled = false;
                }
                else
                {
                    _allVirtualCameras[i].GetComponent<CinemachineVirtualCamera>().enabled = true;
                }
            }
        }
        else
        {
            Debug.LogWarning("No spawn point found for the last scene!");
        }
    }
}