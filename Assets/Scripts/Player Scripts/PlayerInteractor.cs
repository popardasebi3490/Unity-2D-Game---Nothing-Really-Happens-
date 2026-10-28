using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI interactionUIText;

    private InteractableObject currentInteractable = null;

    void Update()
    {
        // Verify if a dialogue is active before allowing interaction
        if (DialogueSystem.Instance.isDialogueActive == true)
        {
            // We hide interaction text prompt
            if (interactionUIText != null) 
            {
                interactionUIText.gameObject.SetActive(false);
            }

            return; // We don't allow interaction while dialogue is active
        }

        // If the player is in range of an interactable object, show the prompt and check for interaction input
        if (currentInteractable != null)
        {
            if (interactionUIText != null)
            {
                interactionUIText.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                
                currentInteractable.TriggerInteraction();

                if (interactionUIText != null)
                {
                    interactionUIText.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            currentInteractable = collision.GetComponent<InteractableObject>();
            if (interactionUIText != null && interactionUIText != null)
            {
                interactionUIText.text = currentInteractable.promptText;
                interactionUIText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            InteractableObject exitingObject = collision.GetComponent<InteractableObject>();
            if (currentInteractable == exitingObject)
            {
                currentInteractable = null;
                if (interactionUIText != null)
                {
                    interactionUIText.gameObject.SetActive(false);
                }
            }
        }
    }
}
