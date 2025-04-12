using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;    // Movement speed (units per second)

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 input;         // Raw input vector from horizontal/vertical axes

    // Called once when the script instance is being loaded
    private void Start()
    {
        // Get the required components from the character GameObject
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb == null)
            Debug.LogError("Missing Rigidbody2D component on the character!");
        if (spriteRenderer == null)
            Debug.LogError("Missing SpriteRenderer component on the character!");

        // Disable gravity as this is a top-down isometric game
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 0f;
    }

    // Called once per frame for input processing and visual updates
    private void Update()
    {
        // Capture input from the Horizontal and Vertical axes (e.g., arrow keys or WASD)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Create and normalize the input vector so diagonal movement isn't faster
        input = new Vector2(horizontal, vertical).normalized;

        // We remove the 45� rotation so that the input remains cardinal (up, down, left, right)
        // Vector2 isoDirection = Quaternion.Euler(0, 0, 45) * input; // Removed for cardinal movement

        // Flip the sprite based on the raw input's X direction.
        if (input.x < 0)
            spriteRenderer.flipX = true;
        else if (input.x > 0)
            spriteRenderer.flipX = false;
    }

    // Called at a fixed interval, ideal for applying physics-based movement
    private void FixedUpdate()
    {
        // Use the raw input vector (without isometric rotation) for cardinal movement.
        rb.linearVelocity = input * moveSpeed;
    }

    // Called after all Update functions have been called. Useful for handling visual changes.
    private void LateUpdate()
    {
        // Dynamically update the sprite's sorting order based on its Y position.
        // This ensures that as the character moves "lower" on the screen, 
        // it renders on top of other sprites.
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }
}
