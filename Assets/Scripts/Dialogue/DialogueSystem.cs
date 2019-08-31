using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

// Attach this to an NPC (or object that can speak)
// Takes an ink file and creates a Story object from it.
// Story object is used by DialogueManager to advance dialogue and populate choices (if any).
public class DialogueSystem : MonoBehaviour
{
    [Tooltip("Name of ink file for dialogue.")]
    public TextAsset inkFile;

    [HideInInspector] public Story story;

    [Tooltip("Rate (in seconds) at which letters are typed out.")]
    public float speed = 0.05f;

    private void Awake()
    {
        story = new Story(inkFile.text);
    }
}
