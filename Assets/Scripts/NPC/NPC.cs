using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum State { Intro, Waiting, Boba, Lobby };
    public State currentState;
    private bool speaking = false;
    private bool endOfDialogue = false;

    private bool hasOrder = false;
    public Boba heldBoba = null;

    [HideInInspector] public DialogueSystem dialogueSystem;
    [HideInInspector] protected DialogueManager dialogueManager;

    private void Awake()
    {
        dialogueSystem = GetComponent<DialogueSystem>();
        dialogueManager = (DialogueManager)FindObjectOfType(typeof(DialogueManager));
    }

    void Start()
    {
        ChangeState(State.Intro);
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Intro:
                IntroUpdate();
                break;
            case State.Waiting:
                WaitingUpdate();
                break;
            case State.Boba:
                BobaUpdate();
                break;
            case State.Lobby:
                LobbyUpdate();
                break;
        }
    }

    private void IntroUpdate()
    {
        if (endOfDialogue)
            ChangeState(State.Waiting);
    }

    private void WaitingUpdate()
    {
        if (endOfDialogue)
            ChangeState(State.Boba);
    }

    private void BobaUpdate()
    {
        if (endOfDialogue)
            ChangeState(State.Lobby);
    }

    private void LobbyUpdate()
    {
        if (endOfDialogue)
            ChangeState(State.Lobby);
    }

    public void Interact(PlayerInteract interactor)
    {
        switch (currentState)
        {
            case State.Intro:
                InteractNoReq(interactor);
                break;
            case State.Waiting:
                InteractReq(interactor);
                break;
            case State.Boba:
                InteractNoReq(interactor);
                break;
            case State.Lobby:
                InteractNoReq(interactor);
                break;
        }
    }

    // Interact without any prerequisites
    private void InteractNoReq(PlayerInteract interactor)
    {
        if (!speaking && !endOfDialogue)
        {
            speaking = true;
            dialogueManager.endDialogueEvent += EndOfDialogue;
            dialogueSystem.ChangeStory(currentState);
            dialogueManager.InitializeDialogue(dialogueSystem);
        }
        else
            dialogueManager.NextSentence();
    }

    // Interact on the condition that the player has an order for the NPC
    private void InteractReq(PlayerInteract interactor)
    {
        if (!hasOrder)
        {
            if (interactor.heldBoba != null && interactor.heldBoba.ready)
            {
                heldBoba = interactor.heldBoba;
                interactor.heldBoba = null;
                hasOrder = true;
                InteractNoReq(interactor);
            }
        }
        else
            InteractNoReq(interactor);
    }

    private void EndOfDialogue(bool disableMovement)
    {
        speaking = disableMovement;
        endOfDialogue = true;
        dialogueManager.endDialogueEvent -= EndOfDialogue;
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
        endOfDialogue = false;
        dialogueSystem.ChangeStory(currentState);
    }

    public void NextState()
    {
        switch (currentState)
        {
            case State.Intro:
                ChangeState(State.Waiting);
                break;
            case State.Waiting:
                ChangeState(State.Boba);
                break;
            case State.Boba:
                ChangeState(State.Lobby);
                break;
        }
    }
    
}
