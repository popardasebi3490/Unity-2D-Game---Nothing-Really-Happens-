using UnityEngine;

public class SetStateOnStart : MonoBehaviour
{
    [Header("What we note in memory when we enter this scene")]
    public string stateID = "Has_Seen_Computer";
    public int valueToSet = 1;

    void Start()
    {
        GameProgress.SetState(stateID, valueToSet);
    }
}
