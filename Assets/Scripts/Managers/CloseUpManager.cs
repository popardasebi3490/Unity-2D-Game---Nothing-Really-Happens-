using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseUpManager : MonoBehaviour
{
    [Header("Unique Identification (Crucial)")]
    [Tooltip("A UNIQUE name for this object, e.g., SwebbyLaptop, LivingRoomDesk")]
    public string uniqueObjectID = "SwebbyLaptop";

    [Header("State Management")]
    [Tooltip("Change this value in the Inspector to test states. It will auto-update visually and globally.")]
    public int currentStateIndex = 0;

    [Header("Scene Navigation")]
    public string returnSceneName = "GoBackRoom";
    public string returnSpawnPointName = "ReturnSpawnPoint"; 

    [Header("Story States (GameObjects)")]
    public GameObject[] storyStates;

    void Start()
    {
        currentStateIndex = GameProgress.GetState(uniqueObjectID);
        UpdateVisualState(currentStateIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (DialogueSystem.Instance != null && DialogueSystem.Instance.isDialogueActive)
                return;

            if (!string.IsNullOrEmpty(returnSpawnPointName))
            {
                GameProgress.targetSpawnPoint = returnSpawnPointName;
            }

            Input.ResetInputAxes(); 
            SceneManager.LoadScene(returnSceneName);
        }
    }

    private void UpdateVisualState(int stateIndex)
    {
        if (storyStates == null) return;

        for (int i = 0; i < storyStates.Length; i++)
        {
            if (storyStates[i] != null)
            {
                storyStates[i].SetActive(i == stateIndex);
            }
        }
    }

    public void AdvanceStoryToState(int newIndex)
    {
        currentStateIndex = newIndex;
        GameProgress.SetState(uniqueObjectID, currentStateIndex);
        UpdateVisualState(currentStateIndex);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (storyStates == null || storyStates.Length == 0) return;

        // Prevent the index from going out of bounds
        currentStateIndex = Mathf.Clamp(currentStateIndex, 0, storyStates.Length - 1);

        // Delay call prevents Unity warnings when using SetActive inside OnValidate
        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this != null) 
            {
                UpdateVisualState(currentStateIndex);
            }
        };
    }
#endif
}