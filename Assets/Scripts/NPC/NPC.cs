using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum State { Intro, Waiting, Lobby, LobbyLoop };
    public State currentState;
    public bool interactable = false;
    protected bool speaking = false;
    protected bool endOfDialogue = false;

    protected bool hasOrder = false;
    public Boba heldBoba = null;

    [HideInInspector] public DialogueSystem dialogueSystem;
    [HideInInspector] protected DialogueManager dialogueManager;

    protected NPCMovement movement;
    protected Vector3 counterPosition;

    [SerializeField] protected Vector3 chairPosition;
    protected bool atDestination = false;
    protected bool walking = false;
    public bool done = false;

    private Animator animator;

    private GameManager gm;

    protected void Awake()
    {
        dialogueSystem = GetComponent<DialogueSystem>();
        dialogueManager = (DialogueManager)FindObjectOfType(typeof(DialogueManager));
        movement = GetComponent<NPCMovement>();
        counterPosition = GameObject.Find("CounterPosition").transform.position;
        gm = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        IntroStart();
        ChangeState(State.Intro);
    }

    private void StateStart()
    {
        switch (currentState)
        {
            case State.Intro:
                IntroStart();
                break;
            case State.Waiting:
                WaitingStart();
                break;
            case State.Lobby:
                LobbyStart();
                break;
            case State.LobbyLoop:
                LobbyLoopStart();
                break;
        }
    }

    protected void Update()
    {
        switch (currentState)
        {
            case State.Intro:
                IntroUpdate();
                break;
            case State.Waiting:
                WaitingUpdate();
                break;
            case State.Lobby:
                LobbyUpdate();
                break;
            case State.LobbyLoop:
                LobbyLoopUpdate();
                break;
        }
    }

    protected virtual void IntroStart()
    {
        interactable = false;
        atDestination = false;
    }

    protected virtual void IntroUpdate()
    {
        if (endOfDialogue)
            ChangeState(State.Waiting);
    }

    protected virtual void WaitingStart()
    {
        interactable = false;
        atDestination = false;
    }

    protected virtual void WaitingUpdate()
    {
        if (atDestination)
            animator.SetLayerWeight(2, 1);
        if (endOfDialogue)
            ChangeState(State.Lobby);
    }

    protected virtual void LobbyStart()
    {
        interactable = true;
        atDestination = false;
    }

    protected virtual void LobbyUpdate()
    {
        if (endOfDialogue)
            ChangeState(State.LobbyLoop);
    }

    protected virtual void LobbyLoopStart()
    {
        interactable = true;
        atDestination = false;
        if (!done)
        {
            done = true;
            gm.FinishNPC();
        }
    }

    protected virtual void LobbyLoopUpdate()
    {
        if (endOfDialogue)
            ChangeState(State.LobbyLoop);
    }

    public void Interact(PlayerInteract interactor)
    {
        if (!interactable)
            return;

        switch (currentState)
        {
            case State.Intro:
                InteractNoReq(interactor);
                break;
            case State.Waiting:
                InteractReq(interactor);
                break;
            case State.Lobby:
                InteractNoReq(interactor);
                break;
            case State.LobbyLoop:
                InteractNoReq(interactor);
                break;
        }
    }

    // Interact without any prerequisites
    protected virtual void InteractNoReq(PlayerInteract interactor)
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
    protected virtual void InteractReq(PlayerInteract interactor)
    {
        if (!hasOrder)
        {
            if (interactor.heldBoba != null && interactor.heldBoba.state == Boba.State.Ready)
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

    protected void EndOfDialogue(bool disableMovement)
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
        StateStart();
    }

    public void NextState()
    {
        switch (currentState)
        {
            case State.Intro:
                ChangeState(State.Waiting);
                break;
            case State.Waiting:
                ChangeState(State.Lobby);
                break;
            case State.Lobby:
                ChangeState(State.LobbyLoop);
                break;
        }
        StateStart();
    }

    protected void StepToDestination(Vector3 destination)
    {
        if (!atDestination && !walking)
        {
            movement.MoveTo(destination);
            movement.destinationReached += ReachedDestination;
            walking = true;
        }
    }

    private void ReachedDestination()
    {
        atDestination = true;
        ResetWalking();
        movement.destinationReached -= ReachedDestination;
    }

    private void ResetWalking()
    {
        walking = false;
        interactable = true;
    }
}
