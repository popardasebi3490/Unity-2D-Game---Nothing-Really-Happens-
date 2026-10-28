using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplayUI : MonoBehaviour
{
    [Header("References")]
    public GameObject inventoryPanel; 
    public Transform slotsContainer; 
    public GameObject itemSlotPrefab; 
    
    public bool IsOpen { get { return isInventoryOpen; } }

    private bool isInventoryOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false);

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.onInventoryChangedEvent += UpdateUI;
        }
    }

    void Update()
    {
        bool isDialogueActive = (DialogueSystem.Instance != null && DialogueSystem.Instance.isDialogueActive);
        
        if (isDialogueActive || GameProgress.isCinematicPlaying)
        {
            if (isInventoryOpen)
            {
                isInventoryOpen = false;
                inventoryPanel.SetActive(false);
            }
            return; 
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryPanel.SetActive(isInventoryOpen);

            if (isInventoryOpen)
            {
                UpdateUI(); 
            }
        }
    }

    void UpdateUI()
    {
        // 1. DISTRUGEM ABSOLUT TOT CE ESTE ÎN CONTAINER (Curățare brutală)
        // Folosim un for invers pentru a evita bug-uri când distrugem elemente dintr-o listă
        for (int i = slotsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(slotsContainer.GetChild(i).gameObject);
        }

        // 2. CONSTRUIM DOAR DACĂ AVEM OBIECTE
        if (InventoryManager.Instance != null && InventoryManager.Instance.collectedItems.Count > 0)
        {
            foreach (string itemName in InventoryManager.Instance.collectedItems)
            {
                // Creează un slot nou pentru fiecare obiect din listă
                GameObject newSlot = Instantiate(itemSlotPrefab, slotsContainer);
                
                Transform iconTransform = newSlot.transform.Find("ItemIcon");
                
                if (iconTransform != null)
                {
                    Image iconImage = iconTransform.GetComponent<Image>();
                    Sprite foundIcon = InventoryManager.Instance.GetItemIcon(itemName);
                    
                    if (foundIcon != null)
                    {
                        iconImage.sprite = foundIcon;
                        
                        // Setăm alfa la 1 ca să se vadă imaginea
                        Color fixedColor = iconImage.color;
                        fixedColor.a = 1f;
                        iconImage.color = fixedColor;
                        iconImage.preserveAspect = true;
                    }
                }
            }
        }
    }
    
    void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.onInventoryChangedEvent -= UpdateUI;
        }
    }
}