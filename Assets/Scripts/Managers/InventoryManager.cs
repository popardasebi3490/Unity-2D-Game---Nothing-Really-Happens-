using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string itemName;
    public Sprite itemIcon;
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Items Database")]
    public List<ItemData> itemDatabase = new List<ItemData>();

    [HideInInspector]
    public List<string> collectedItems = new List<string>();

    public delegate void OnInventoryChanged();
    public event OnInventoryChanged onInventoryChangedEvent;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(transform.root.gameObject);
        }
    }

    public void AddItem(string itemNameToAdd)
    {
        collectedItems.Add(itemNameToAdd);
        Debug.Log("Item added: " + itemNameToAdd);
        
        if (onInventoryChangedEvent != null)
        {
            onInventoryChangedEvent.Invoke();
        }
    }

    public bool HasItem(string itemToCheck)
    {
        return collectedItems.Contains(itemToCheck);
    }

    public void RemoveItem(string itemToRemove)
    {
        if (collectedItems.Contains(itemToRemove))
        {
            collectedItems.Remove(itemToRemove);

            if (onInventoryChangedEvent != null)
            {
                onInventoryChangedEvent.Invoke();
            }
        }
    }

    public Sprite GetItemIcon(string searchItemName)
    {
        foreach (ItemData data in itemDatabase)
        {
            if (data.itemName == searchItemName)
            {
                return data.itemIcon;
            }
        }
        return null; 
    }
}