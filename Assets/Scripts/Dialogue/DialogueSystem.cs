using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

// Attach this to an NPC (or object that can speak)
// Takes an ink file and creates a Story object from it.
// Story object is used by DialogueManager to advance dialogue and populate choices (if any).
public class DialogueSystem : MonoBehaviour
{
    [Tooltip("Name of ink file for starting dialogue.")]
    public TextAsset inkFileIntro;

    [Tooltip("Name of ink file for waiting dialogue.")]
    public TextAsset inkFileWaiting;

    [Tooltip("Name of ink file for dialogue after receiving boba.")]
    public TextAsset inkFileBoba;

    [Tooltip("Name of ink file for lobby dialogue.")]
    public TextAsset inkFileLobby;

    [HideInInspector] public Story story;

    [Tooltip("Rate (in seconds) at which letters are typed out.")]
    public float speed = 0.05f;

    public void ChangeStory(NPC.State state)
    {
        switch (state)
        {
            case NPC.State.Intro:
                story = new Story(inkFileIntro.text);
                break;
            case NPC.State.Waiting:
                story = new Story(inkFileWaiting.text);
                break;
            case NPC.State.Boba:
                story = new Story(inkFileBoba.text);
                break;
            case NPC.State.Lobby:
                story = new Story(inkFileLobby.text);
                break;
        }
    }
}
