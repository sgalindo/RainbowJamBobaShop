using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interaction script for player.
// Attach to PlayerInteract object.
// Interact button: E or Spacebar.
// TODO: Handle interaction with machines.
public class PlayerInteract : MonoBehaviour
{
    public GameObject interactable; // Reference to interactable object that is close enough to interact
    private DialogueManager dm;

    private void Start()
    {
        dm = (DialogueManager)FindObjectOfType(typeof(DialogueManager));
    }

    // When something enters trigger collider, check if it is interactable.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NPC")
            interactable = other.gameObject;
    }

    // When something exits trigger collider, if it is interactable, clear its reference
    // TODO: Account for multiple interactables in collider at once
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "NPC")
            interactable = null;
    }

    private void Update()
    {
        // "E" or "Spacebar"
        if (Input.GetButtonDown("Interact"))
        {
            // If not speaking to anyone
            if (!dm.speaking)
            {
                // Check if object we'r einteracting with can be spoken to
                DialogueSystem ds = interactable.GetComponent<DialogueSystem>();
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
}
