using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    [Header("UI Elements")]
    public GameObject dialogueBoxPanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;

    [Header("Settings")]
    public float typingSpeed = 0.05f;

    public bool isDialogueActive = false;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentFullText = "";
    private float blockInputTimer = 0f;

    private Queue<string> dialoguePages = new Queue<string>(); 

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialogueBoxPanel.SetActive(false);
    }

    public void ShowDialogue(string speakerName, string[] textPages)
    {
        isDialogueActive = true;
        speakerNameText.text = speakerName;
        
        dialoguePages.Clear(); 
        
        foreach (string page in textPages)
        {
            dialoguePages.Enqueue(page);
        }

        dialogueBoxPanel.SetActive(true);
        DisplayNextPage(); 
    }

    private void DisplayNextPage()
    {
        if (dialoguePages.Count == 0)
        {
            HideDialogue();
            return;
        }

        currentFullText = dialoguePages.Dequeue(); 
        
        isTyping = true; 
        blockInputTimer = 0.1f; 

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypewriterEffect());
    }

    public void HideDialogue()
    {
        isDialogueActive = false;
        dialogueBoxPanel.SetActive(false);
        isTyping = false;
    }

    public bool IsDialogueFinished()
    {
        return !isDialogueActive;
    }

    void Update()
    {
        if (blockInputTimer > 0f)
        {
            blockInputTimer -= Time.deltaTime;
            return; 
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                dialogueText.text = currentFullText;
                isTyping = false;
                blockInputTimer = 0.1f; 
            }
            else if (dialogueBoxPanel.activeSelf)
            {
                DisplayNextPage();
            }
        }
    }

    private IEnumerator TypewriterEffect()
    {
        dialogueText.text = "";

        foreach (char letter in currentFullText.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false; 
    }
}