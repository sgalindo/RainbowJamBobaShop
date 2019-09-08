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

    [Tooltip("Name of ink file for dialogue after receiving boba.")]
    public TextAsset inkFileWaiting;

    [Tooltip("Name of ink file for lobby dialogue.")]
    public TextAsset inkFileLobby;

    [Tooltip("Name of ink file for LOOPING lobby dialogue.")]
    public TextAsset inkFileLobbyLoop;

    [HideInInspector] public Story story;

    [Tooltip("Rate (in seconds) at which letters are typed out.")]
    public float speed = 0.05f;

    public bool ChangeStory(NPC.State state)
    {
        switch (state)
        {
            case NPC.State.Intro:
                story = new Story(inkFileIntro.text);
                break;
            case NPC.State.Waiting:
                if (inkFileWaiting == null) return false;
                story = new Story(inkFileWaiting.text);
                break;
            case NPC.State.Lobby:
                story = new Story(inkFileLobby.text);
                break;
            case NPC.State.LobbyLoop:
                story = new Story(inkFileLobbyLoop.text);
                break;
        }
        return true;
    }

    public void CreateStory(string txt)
    {
        story = new Story(txt);
    }
}
