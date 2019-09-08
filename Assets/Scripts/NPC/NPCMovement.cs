using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public delegate void MovementDelegate();
    public event MovementDelegate destinationReached = delegate { };

    private Transform destination;
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
        if (moving && destination != null)
        {
            if (transform.position != destination.position)
            {
                PlayMoveAnimation();
                transform.position = Vector2.MoveTowards(transform.position, destination.position, moveSpeed);
            }
            else
            {
                moving = false;
                destination = null;
                animator.SetLayerWeight(1, 0);
                destinationReached();
            }
        }
    }

    public void MoveTo(Transform dest)
    {
        destination = dest;
        moving = true;
        moveDirection = new Vector2(dest.position.x - transform.position.x, dest.position.y - transform.position.y).normalized;
    }

    private void PlayMoveAnimation()
    {
        animator.SetFloat("DirectionX", moveDirection.x);
        animator.SetFloat("DirectionY", moveDirection.y);
        animator.SetLayerWeight(1, 1);
    }
}
