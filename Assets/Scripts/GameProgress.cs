using System.Collections.Generic;

public static class GameProgress
{
    public static bool hasSeenIntro = false; 
    public static bool isCinematicPlaying = false; 
    
    public static string targetSpawnPoint = "";

    public static List<string> lootedObjects = new List<string>();

    private static Dictionary<string, int> objectStates = new Dictionary<string, int>();

    public static void SetState(string objectID, int stateIndex)
    {
        if (objectStates.ContainsKey(objectID))
        {
            objectStates[objectID] = stateIndex; 
        }
        else
        {
            objectStates.Add(objectID, stateIndex); 
        }
    }

    public static int GetState(string objectID)
    {
        if (objectStates.ContainsKey(objectID))
        {
            return objectStates[objectID]; 
        }
        else
        {
            return 0; 
        }
    }

    [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        hasSeenIntro = false;
        isCinematicPlaying = false;
        targetSpawnPoint = "";
        objectStates.Clear(); 
        
        lootedObjects.Clear(); 
    }
}