using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interaction script for player.
// Attach to PlayerInteract object.
// Interact button: E or Spacebar.
// TODO: Handle interaction with machines.
public class PlayerInteract : MonoBehaviour
{
    public List<GameObject> interactables; // List of objects we can currently interact with
    private DialogueManager dm;

    private void Start()
    {
        dm = (DialogueManager)FindObjectOfType(typeof(DialogueManager));
        interactables = new List<GameObject>();
    }

    private void Update()
    {
        // "E" or "Spacebar"
        if (Input.GetButtonDown("Interact"))
        {
            // If not speaking to anyone
            if (!dm.speaking)
            {
                DialogueSystem ds = null;

                // Check if closest object (to player's field of view) can be spoken to
                if (interactables.Count > 0)
                {
                    int index = FindClosestToFOV();
                    ds = interactables[index].transform.parent.GetComponent<DialogueSystem>();
                    interactables.RemoveAt(index); // Remove object since we've already started interacting with it
                }
                if (ds)
                {
                    // If so, begin dialogue
                    dm.OpenDialogue(ds);
                }
            }
            else
            {
                // If already speaking, continue dialogue
                dm.NextSentence();
            }
        }
    }

    // When something enters trigger collider, check if it is interactable.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NPC")
            interactables.Add(other.gameObject);
    }

    // When something exits trigger collider, if it is interactable, clear its reference
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "NPC")
            interactables.Remove(other.gameObject);
    }

    // Finds closest interactable object.
    private int FindClosest()
    {
        int closest = 0;
        float minDistance = 10000f;
        for (int i = 0; i < interactables.Count; i++)
        {
            if (Vector2.Distance(transform.position, interactables[i].transform.position) < minDistance)
                closest = i;
        }
        return closest;
    }

    // Finds object that is closest to player's Field Of View
    // Highest being directly in front of player, lowest directly behind.
    private int FindClosestToFOV()
    {
        int closest = 0;
        float minProduct = -1f;
        for (int i = 0; i < interactables.Count; i++)
        {
            Vector2 playerToObject = interactables[i].transform.position - transform.position;
            if (Vector2.Dot(transform.forward, playerToObject) >= minProduct)
                closest = i;
        }
        return closest;
    }

}
