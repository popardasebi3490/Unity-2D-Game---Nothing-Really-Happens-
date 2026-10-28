using UnityEngine;

public class ShowIfGameState : MonoBehaviour
{
    [Header("Conditions for this object to be active")]
    [Tooltip("Ex: Has_Seen_Computer")]
    public string requiredStateID;

    public int requiredValue = 1; 

    void Start()
    {
        if (GameProgress.GetState(requiredStateID) >= requiredValue)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
