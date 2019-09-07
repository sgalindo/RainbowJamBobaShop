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
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        movementInput = Vector2.zero;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we are currently speaking. If so, don't take movement input.
        if (!PlayerInteract.interacting)
        {
            // Store input into vector and clamp magnitude to 1 (to prevent faster diagonal movement)
            movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            movementInput = Vector2.ClampMagnitude(movementInput, 1);
        }
        else
            movementInput = Vector2.zero; // Else reset movementInput to avoid slidies after interacting with something
    }

    private void FixedUpdate()
    {
        // Only move if there is input
        if (movementInput != Vector2.zero)
        {
            MovePlayer();
            PlayMoveAnimation();
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetLayerWeight(1, 0);
        }
    }

    // Moves player by setting its Rigidbody2D's velocity to its input vector (direction) * moveSpeed
    private void MovePlayer()
    {
        rb.velocity = movementInput * moveSpeed;
    }

    private void PlayMoveAnimation()
    {
        animator.SetFloat("DirectionX", movementInput.x);
        animator.SetFloat("DirectionY", movementInput.y);
        animator.SetLayerWeight(1, 1);
    }
}
