using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start() 
    {
        if (!string.IsNullOrEmpty(GameProgress.targetSpawnPoint))
        {
            GameObject spawnPoint = GameObject.Find(GameProgress.targetSpawnPoint);

            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
            }
            else
            {
                Debug.LogWarning("Spawn point not found: " + GameProgress.targetSpawnPoint);
            }

            GameProgress.targetSpawnPoint = "";
        }
    }
}