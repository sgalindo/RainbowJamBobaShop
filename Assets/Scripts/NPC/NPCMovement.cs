using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public delegate void MovementDelegate();
    public event MovementDelegate destinationReached = delegate { };

    private Transform destination;
    public float moveSpeed;

    private bool moving = true;

    private void FixedUpdate()
    {
        if (moving && destination != null)
        {
            if (transform.position != destination.position)
                transform.position = Vector2.MoveTowards(transform.position, destination.position, moveSpeed);
            else
            {
                moving = false;
                destination = null;
                destinationReached();
            }
        }
    }

    public void MoveTo(Transform dest)
    {
        destination = dest;
        moving = true;
    }
}
