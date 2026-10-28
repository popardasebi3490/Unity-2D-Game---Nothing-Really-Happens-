using UnityEngine;
using System.Collections;

public class NewGameCinematic : MonoBehaviour
{
    [Header("Cinematic Settings")]
    public Transform bedSpawnPoint;
    public Transform floorTarget;
    public Transform visualChild;
    
    public float jumpDuration = 0.5f;
    public int bedBounces = 4;
    
    public float minBounceHeight = 1.6f;
    public float maxBounceHeight = 2.0f;
    public float bounceDuration = 0.3f;

    [Header("Squash/Landing Settings")]
    public float baseSquashAmount = 0.3f;
    public float squashDuration = 0.1f;

    [Header("Debug / Testing")]
    public bool forceTestAnimation = false;

    [Header("Dialogue Content")]
    public string myName = "Swebby"; 
    
    [TextArea(2, 4)]
    public string[] firstStartText = { "I'm so bored of being unemployed!" };
    
    [TextArea(2, 4)]
    public string[] secondStartText = { "I'll have to send some more applications I guess..." };

    private PlayerMovement playerMovement;
    private Vector3 originalVisualScale;

    void Awake()
    {
        if (visualChild != null)
        {
            originalVisualScale = visualChild.localScale;
        }
    }

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        Collider2D playerCollider = GetComponent<Collider2D>();

        if (forceTestAnimation)
        {
            GameProgress.hasSeenIntro = false;
        }

        if (GameProgress.hasSeenIntro == false)
        {   
            if (bedSpawnPoint != null)
            {
                transform.position = bedSpawnPoint.position;
            }

            StartCoroutine(PlayIntroCinematic());
        }
        else 
        {
            if (DialogueSystem.Instance != null)
            {
                DialogueSystem.Instance.HideDialogue();
            }
            
            if (playerMovement != null) playerMovement.enabled = true;
            if (playerCollider != null) playerCollider.enabled = true;
        }
    }

    private IEnumerator PlayIntroCinematic()
    {        GameProgress.isCinematicPlaying = true;
        
        if (playerMovement != null) playerMovement.enabled = false;
        
        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null) playerCollider.enabled = false;

        yield return new WaitForSeconds(0.3f);

        Vector3 visualLocalPos = visualChild.localPosition;

        for (int i = 0; i < bedBounces; i++)
        {
            float randomHeight = Random.Range(minBounceHeight, maxBounceHeight);
            float bounceTime = 0f;

            while (bounceTime < bounceDuration)
            {
                float verticalOffset = Mathf.Sin((bounceTime / bounceDuration) * Mathf.PI) * randomHeight;
                Vector3 currentPos = visualLocalPos;
                currentPos.y += verticalOffset;
                visualChild.localPosition = currentPos;

                bounceTime += Time.deltaTime;
                yield return null;
            }

            visualChild.localPosition = visualLocalPos;
            yield return StartCoroutine(SquashLandingEffect(randomHeight));
        }

        yield return new WaitForSeconds(0.5f);

        DialogueSystem.Instance.ShowDialogue(myName, firstStartText);
        while (DialogueSystem.Instance.isDialogueActive) yield return null;
        
        yield return new WaitForSeconds(0.5f);

        Vector3 rootStartPos = transform.position;
        Vector3 rootTargetPos = floorTarget.position; 
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector3.Lerp(rootStartPos, rootTargetPos, elapsedTime / jumpDuration);
            
            float verticalOffset = Mathf.Sin((elapsedTime / jumpDuration) * Mathf.PI) * 0.5f;
            Vector3 currentVisualPos = visualLocalPos;
            currentVisualPos.y += verticalOffset; 
            visualChild.localPosition = currentVisualPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = rootTargetPos;
        visualChild.localPosition = visualLocalPos;

        yield return StartCoroutine(SquashLandingEffect(0.5f));

        DialogueSystem.Instance.ShowDialogue(myName, secondStartText);
        while (DialogueSystem.Instance.isDialogueActive) yield return null;

        if (playerMovement != null) playerMovement.enabled = true;
        if (playerCollider != null) playerCollider.enabled = true;

        GameProgress.hasSeenIntro = true;
        GameProgress.isCinematicPlaying = false; 
        
        Destroy(this);
    }

    private IEnumerator SquashLandingEffect(float fallHeight)
    {
        float timer = 0f;
        float squashMultiplier = fallHeight / maxBounceHeight;
        float actualSquashAmount = baseSquashAmount * squashMultiplier; 
        
        Vector3 squashedScale = new Vector3(
            originalVisualScale.x * (1f + actualSquashAmount), 
            originalVisualScale.y * (1f - actualSquashAmount), 
            originalVisualScale.z
        );

        while (timer < squashDuration)
        {
            float progress = timer / squashDuration;
            visualChild.localScale = Vector3.Lerp(originalVisualScale, squashedScale, progress);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;
        while (timer < squashDuration)
        {
            float progress = timer / squashDuration;
            visualChild.localScale = Vector3.Lerp(squashedScale, originalVisualScale, progress);
            timer += Time.deltaTime;
            yield return null;
        }

        visualChild.localScale = originalVisualScale; 
    }
}