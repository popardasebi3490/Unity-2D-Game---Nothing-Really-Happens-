using UnityEngine;
using UnityEngine.Events;
using System.Collections; 

public class InteractableObject : MonoBehaviour
{
    [Header("Save Settings")]
    [Tooltip("Pune un nume UNIC pentru fiecare obiect, ex: Sertar_Birou_1")]
    public string uniqueID; 
    public bool saveState = true;

    [Header("Interaction Texts")]
    public string promptText = "Press [F] to interact"; 
    
    [Header("Dialogue Settings")]
    public bool showDialogue = true;
    public string speakerName = "Void"; 
    
    [TextArea(2, 4)] 
    public string[] reactionMessages = { "It's just a drawer." }; 
    
    [TextArea(2, 4)]
    public string[] emptyMessages; 

    [Header("Lock & Key Settings (What you NEED)")]
    public string[] requiredItemNames;
    [TextArea(2, 4)]
    public string[] lockedMessages = { "It's locked or I don't have all required items." };
    [TextArea(2, 4)]
    public string[] unlockMessages = { "I have everything I need!" };
    public bool consumeKeys = true;

    [Header("Item Settings (What you GET)")]
    public bool isItemPickup = false;
    public string[] itemNamesToGive;
    public bool destroyAfterPickup = true;

    [Header("Custom Actions")]
    public UnityEvent onInteract;

    private bool hasBeenLooted = false; 

    void Start()
    {
        if (saveState && !string.IsNullOrEmpty(uniqueID) && GameProgress.lootedObjects.Contains(uniqueID))
        {
            hasBeenLooted = true;
        }

        if (isItemPickup && destroyAfterPickup && InventoryManager.Instance != null)
        {
            if (itemNamesToGive != null && itemNamesToGive.Length > 0 && InventoryManager.Instance.HasItem(itemNamesToGive[0]))
            {
                gameObject.SetActive(false); 
            }
            else if (hasBeenLooted)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void TriggerInteraction()
    {
        StartCoroutine(InteractionSequence());
    }

    private IEnumerator InteractionSequence()
    {
        string[] messagesToPlay = reactionMessages;
        bool isSuccess = true; 
        
        if (hasBeenLooted && emptyMessages.Length > 0)
        {
            messagesToPlay = emptyMessages;
            isSuccess = false; 
        }
        else if (requiredItemNames != null && requiredItemNames.Length > 0)
        {
            bool hasAllKeys = true;
            
            if (InventoryManager.Instance != null)
            {
                foreach(string reqItem in requiredItemNames)
                {
                    if (!InventoryManager.Instance.HasItem(reqItem))
                    {
                        hasAllKeys = false;
                        break; 
                    }
                }
            }
            else
            {
                hasAllKeys = false;
            }

            if (hasAllKeys)
            {
                messagesToPlay = unlockMessages;
                
                if (consumeKeys && InventoryManager.Instance != null)
                {
                    foreach(string reqItem in requiredItemNames)
                    {
                        InventoryManager.Instance.RemoveItem(reqItem);
                    }
                }
            }
            else
            {
                messagesToPlay = lockedMessages;
                isSuccess = false; 
            }
        }

        if (showDialogue && DialogueSystem.Instance != null && messagesToPlay.Length > 0)
        {
            DialogueSystem.Instance.ShowDialogue(speakerName, messagesToPlay);
            
            while (DialogueSystem.Instance.isDialogueActive)
            {
                yield return null;
            }
        }
        else if (showDialogue && DialogueSystem.Instance == null)
        {
            Debug.LogWarning("No DialogueSystem found!");
        }

        if (isSuccess && !hasBeenLooted)
        {
            if (isItemPickup && InventoryManager.Instance != null && itemNamesToGive != null && itemNamesToGive.Length > 0)
            {
                foreach(string itemToGive in itemNamesToGive)
                {
                    if (!string.IsNullOrEmpty(itemToGive))
                    {
                        InventoryManager.Instance.AddItem(itemToGive);
                    }
                }
            }
            
            hasBeenLooted = true; 

            if (saveState && !string.IsNullOrEmpty(uniqueID))
            {
                if (!GameProgress.lootedObjects.Contains(uniqueID))
                {
                    GameProgress.lootedObjects.Add(uniqueID);
                }
            }
        }

        if (onInteract != null && isSuccess) 
        {
            onInteract.Invoke();
        }

        if (isSuccess && isItemPickup && destroyAfterPickup)
        {
            gameObject.SetActive(false); 
        }
    }
}