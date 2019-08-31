using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles all player movement.
// Attach to player.
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 movementInput; // Input vector. Holds both horizontal and vertical inputs
    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    private DialogueManager dm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        dm = (DialogueManager)FindObjectOfType(typeof(DialogueManager));
        movementInput = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we are currently speaking. If so, don't take movement input.
        if (!dm.speaking)
        {
            // Store input into vector and clamp magnitude to 1 (to prevent faster diagonal movement)
            movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            movementInput = Vector2.ClampMagnitude(movementInput, 1);
        }
    }

    private void FixedUpdate()
    {
        // Only move if there is input
        if (movementInput != Vector2.zero)
        {
            MovePlayer();
            FlipPlayer();
        }
    }

    // Moves player by setting its Rigidbody2D's velocity to its input vector (direction) * moveSpeed
    private void MovePlayer()
    {
        rb.velocity = movementInput * moveSpeed;
    }

    // Flips player sprite if moving the to the left.
    private void FlipPlayer()
    {
        if (movementInput.x < 0f)
            playerSprite.flipX = true;
        else if (movementInput.x > 0f)
            playerSprite.flipX = false;
    }
}
