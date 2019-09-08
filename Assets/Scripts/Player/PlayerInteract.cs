using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interaction script for player.
// Attach to PlayerInteract object.
// Interact button: E or Spacebar.
public class PlayerInteract : MonoBehaviour
{
    public List<GameObject> interactables; // List of objects we can currently interact with
    private DialogueManager dm;
    public Boba heldBoba = null;
    public static bool interacting = false;
    private NPC npc = null;
    private Machine machine = null;

    private void Start()
    {
        dm = (DialogueManager)FindObjectOfType(typeof(DialogueManager));
        interactables = new List<GameObject>();
        dm.beginDialogueEvent += SetMovement;
        dm.endDialogueEvent += SetMovement;
    }

    private void Update()
    {
        // "E" or "Spacebar"
        if (Input.GetButtonDown("Interact"))
            Interact();
    }

    private void Interact()
    {
        // If not interacting with anything
        if (!interacting)
        {
            // Check if closest object (to player's field of view) can be interacted with
            if (interactables.Count > 0)
            {
                int index = FindClosestToFOV();
                npc = interactables[index].transform.parent.GetComponent<NPC>();
                machine = interactables[index].transform.parent.GetComponent<Machine>();
                interactables.RemoveAt(index); // Remove object since we've already started interacting with it
            }
        }

        // Interact depending on if interactable is NPC or machine
        if (npc)
        {
            npc.Interact(this);
        }
        else if (machine)
        {
            interacting = machine.Interact(this);
            machine = null;
        }
    }

    // When something enters trigger collider, check if it is interactable.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NPC" || other.tag == "Machine")
            interactables.Add(other.gameObject);
    }

    // When something exits trigger collider, if it is interactable, clear its reference
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "NPC" || other.tag == "Machine")
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

    private void SetMovement(bool enabled)
    {
        interacting = enabled;
        if (!enabled)
            npc = null;
    }
}
