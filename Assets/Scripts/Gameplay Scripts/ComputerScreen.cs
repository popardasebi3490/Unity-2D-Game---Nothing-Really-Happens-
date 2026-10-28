using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ComputerScreen : MonoBehaviour
{
    [Header("Player Settings")]
    public string playerName = "Void";

    [Header("Job 1 Settings")]
    [TextArea(2, 4)]
    public string[] job1Dialogues = { 
        "\"Hello! I would like to apply for this job.\"", 
        "\"I have zero experience, but I learn fast.\"",
        "Done... I sent it. I hope they don't call me." 
    };
    public GameObject applyButton1;

    [Header("Job 2 Settings")]
    [TextArea(2, 4)]
    public string[] job2Dialogues = { 
        "\"Competitive salary? Does that mean I won't starve?\"", 
        "Whatever, applying here too." 
    };
    public GameObject applyButton2;

    [Header("Job 3 Settings")]
    [TextArea(2, 4)]
    public string[] job3Dialogues = { 
        "I'm cooked."
    };
    public GameObject applyButton3;

    private CanvasGroup uiGroup;

    void Start()
    {
        uiGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (DialogueSystem.Instance != null)
        {
            uiGroup.interactable = !DialogueSystem.Instance.isDialogueActive;
        }
    }

    public void ClosePC()
    {
        if (DialogueSystem.Instance != null && DialogueSystem.Instance.isDialogueActive)
        {
            return;
        }
        
        gameObject.SetActive(false);
    }

    public void ApplyForJob1()
    {
        if (DialogueSystem.Instance != null && !DialogueSystem.Instance.isDialogueActive)
        {
            DialogueSystem.Instance.ShowDialogue(playerName, job1Dialogues);
            
            if (applyButton1 != null)
            {
                applyButton1.SetActive(false);
            }
        }
    }

    public void ApplyForJob2()
    {
        if (DialogueSystem.Instance != null && !DialogueSystem.Instance.isDialogueActive)
        {
            DialogueSystem.Instance.ShowDialogue(playerName, job2Dialogues);
            
            if (applyButton2 != null)
            {
                applyButton2.SetActive(false);
            }
        }
    }

    public void ApplyForJob3()
    {
        if (DialogueSystem.Instance != null && !DialogueSystem.Instance.isDialogueActive)
        {
            DialogueSystem.Instance.ShowDialogue(playerName, job3Dialogues);
            
            if (applyButton3 != null)
            {
                applyButton3.SetActive(false);
            }
        }
    }
    
}
