using UnityEngine; 

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Flip Animation Settings")]
    public float flipDuration = 0.15f;
    private float currentFlipTime = 0f;
    private bool isFlipping = false;
    private float startFlipX;
    private float targetFlipX;

    [Header("Squash and Stretch Settings")]
    public float animationSpeed = 10f; 
    public float squashAmount = 0.15f; 

    [Header("References")]
    public Transform graphicsTransform; 

    private Rigidbody2D rb;
    private float horizontalInput;
    private float verticalInput;
    
    private Vector3 originalScale;
    private float wobbleTimer = 0f;
    private bool isFacingRight = true;

    private InventoryDisplayUI inventoryUI;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        inventoryUI = FindAnyObjectByType<InventoryDisplayUI>();
        
        if (graphicsTransform != null)
        {
            originalScale = graphicsTransform.localScale;
        }
    }

    void Update()
    {
        // 1. Check if dialogue or inventory is active
        bool isDialogueActive = (DialogueSystem.Instance != null && DialogueSystem.Instance.isDialogueActive);
        bool isInventoryOpen = (inventoryUI != null && inventoryUI.IsOpen);

        if (isDialogueActive || isInventoryOpen)
        {
            horizontalInput = 0f;
            verticalInput = 0f;

            if (isFlipping)
            {
                isFlipping = false;
                if (graphicsTransform != null)
                {
                    graphicsTransform.localScale = new Vector3(targetFlipX, originalScale.y, originalScale.z);
                }
            }

            HandleWobbleAnimation(); 
            return;
        }

        // 2. Read inputs 
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // 3. Handle flipping logic
        if (horizontalInput > 0f && !isFacingRight && !isFlipping)
        {
            StartSmoothFlip(true);
        }
        else if (horizontalInput < 0f && isFacingRight && !isFlipping)
        {
            StartSmoothFlip(false);
        }

        // 4. Process the active animation
        if (isFlipping)
        {
            HandleSmoothFlip();
        }
        else
        {
            HandleWobbleAnimation();
        }        
    }

    void FixedUpdate()
    {
        bool isDialogueActive = (DialogueSystem.Instance != null && DialogueSystem.Instance.isDialogueActive);
        bool isInventoryOpen = (inventoryUI != null && inventoryUI.IsOpen);

        if (isDialogueActive || isInventoryOpen)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, verticalInput * moveSpeed);
    }

    private void StartSmoothFlip(bool facingRight)
    {
        isFlipping = true;
        isFacingRight = facingRight;
        currentFlipTime = 0f;

        startFlipX = originalScale.x;
        targetFlipX = originalScale.x * -1f;
        originalScale.x = targetFlipX;
    }

    private void HandleSmoothFlip()
    {
        currentFlipTime += Time.deltaTime;
        float progress = currentFlipTime / flipDuration;

        float currentXScale = Mathf.Lerp(startFlipX, targetFlipX, progress);
        
        if (graphicsTransform != null)
        {
            graphicsTransform.localScale = new Vector3(currentXScale, originalScale.y, originalScale.z);
        }

        if (progress >= 1f)
        {
            isFlipping = false;       
        }
    }

    private void HandleWobbleAnimation()
    {
        if (graphicsTransform == null) return;

        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            wobbleTimer += Time.deltaTime * animationSpeed;

            float squashY = Mathf.Sin(wobbleTimer) * squashAmount;
            float stretchX = Mathf.Cos(wobbleTimer) * (squashAmount / 2f); 

            graphicsTransform.localScale = new Vector3(
                originalScale.x + stretchX,
                originalScale.y + squashY,
                originalScale.z
            );
        }
        else
        {
            wobbleTimer = 0f;
            graphicsTransform.localScale = Vector3.Lerp(graphicsTransform.localScale, originalScale, Time.deltaTime * 10f);
        }
    }
}