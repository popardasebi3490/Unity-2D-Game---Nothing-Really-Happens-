using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadHelper : MonoBehaviour
{
    [Tooltip("The name of the spawn point in the next scene where the player should appear.")]
    public string spawnPointNameForNextScene;

    public void LoadTargetScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(spawnPointNameForNextScene))
        {
            GameProgress.targetSpawnPoint = spawnPointNameForNextScene;
        }

        Input.ResetInputAxes();

        SceneManager.LoadScene(sceneName);
    }
}
