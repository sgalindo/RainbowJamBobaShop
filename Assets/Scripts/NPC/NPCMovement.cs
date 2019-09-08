using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public delegate void MovementDelegate();
    public event MovementDelegate destinationReached = delegate { };

    private Vector3 destination;
    public float moveSpeed;
    private Animator animator;

    private Vector2 moveDirection;

    private bool moving = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (moving && destination != Vector3.zero)
        {
            if (transform.position != destination)
            {
                PlayMoveAnimation();
                transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed);
            }
            else
            {
                moving = false;
                destination = Vector3.zero;
                animator.SetLayerWeight(1, 0);
                destinationReached();
            }
        }
    }

    public void MoveTo(Vector3 dest)
    {
        destination = dest;
        moving = true;
        moveDirection = new Vector2(dest.x - transform.position.x, dest.y - transform.position.y).normalized;
    }

    private void PlayMoveAnimation()
    {
        animator.SetFloat("DirectionX", moveDirection.x);
        animator.SetFloat("DirectionY", moveDirection.y);
        animator.SetLayerWeight(1, 1);
    }
}
