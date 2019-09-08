using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// This script will handle dialogue display and choices.
// Basically interprets a DialogueSystem and sets the UI text accordingly.
public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; 
    public GameObject choicePanel;
    public GameObject buttonPrefab;
    private EventSystem eventSystem;
    private TextMeshProUGUI displayText;
    private Story story;
    private float speed;
    private List<ChoiceButton> choices;
    private bool isTyping;

    public delegate void BeginDialogueEvent(bool disableMovement);
    public event BeginDialogueEvent beginDialogueEvent = delegate { };

    public delegate void EndDialogueEvent(bool disableMovement);
    public event EndDialogueEvent endDialogueEvent = delegate { };

    private void Awake()
    {
        // Set reference to dialogue text and disable all panels on awake.
        displayText = dialoguePanel.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        eventSystem = (EventSystem)FindObjectOfType(typeof(EventSystem));
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
        choices = new List<ChoiceButton>();
        isTyping = false;
    }

    public void Update()
    {

    }

    public void InitializeDialogue(DialogueSystem ds)
    {
        if (story == null)
        {
            story = ds.story;
            speed = ds.speed;
            dialoguePanel.SetActive(true);
            displayText.text = "";
            beginDialogueEvent(true);
            NextSentence();
        }
    }

    public bool Interact(PlayerInteract interactor)
    {
        return NextSentence();
    }

    // Continue the story. 
    // Either gets the next available sentence, 
    // displays choices,
    // or closes the dialogue at the end of the story.
    public bool NextSentence()
    {
        // If currently typing, stop typing and display whole sentence.
        if (isTyping)
        {
            ResetText();
            displayText.text = story.currentText;
        }
        // canContinue returns true only if it has another sentence in queue
        // (no choices, and not at end of story)
        else if (story.canContinue)
        {
            ResetText();
            StartCoroutine(Type(story.Continue()));
        }
        // Else, display choices if there are any.
        else if (story.currentChoices.Count > 0 && choices.Count == 0)
        {
            PopulateChoices();
        }
        // Otherwise, we're at end of story and we can close the dialogue.
        else if (choices.Count == 0)
        {
            CloseDialogue();
            return false;
        }
        return true;
    }

    // Types out the current sentence letter by letter at the currentSystem's speed
    private IEnumerator Type(string sentence)
    {
        isTyping = true;
        foreach (char letter in sentence.ToCharArray())
        {
            displayText.text += letter;
            yield return new WaitForSeconds(speed);
        }
        isTyping = false;
    }

    private void ResetText()
    {
        StopAllCoroutines();
        isTyping = false;
        displayText.text = "";
    }

    // Stop typing if still typing a sentence and remove references
    private void CloseDialogue()
    {
        ResetText();
        endDialogueEvent(false);
        story = null;
        dialoguePanel.SetActive(false);
    }

    // Instantiates buttons for each available choice in the current point in the story
    private void PopulateChoices()
    {
        choicePanel.SetActive(true);

        for (int i = 0; i < story.currentChoices.Count; i++)
        {
            GameObject btn = Instantiate(buttonPrefab, choicePanel.transform);
            TextMeshProUGUI gui = btn.transform.Find("ChoiceText").GetComponent<TextMeshProUGUI>();
            gui.text = story.currentChoices[i].text;
            gui.enableAutoSizing = false;
            gui.enableAutoSizing = true;
            ChoiceButton choiceBtn = btn.GetComponent<ChoiceButton>();
            choiceBtn.index = i; // Set button's choice index
            choiceBtn.buttonAction += SelectButton; // Subscribe to buttonAction event

            choices.Add(choiceBtn);
        }
        eventSystem.SetSelectedGameObject(choices[0].gameObject);
    }

    public void SelectButton(int index)
    {
        story.ChooseChoiceIndex(index); // Pick a index choice from story

        // Clear all choices' events to prevent memory leaks and destroy buttons.
        foreach (ChoiceButton choice in choices)
        {
            choice.buttonAction -= SelectButton;
            Destroy(choice.gameObject);
        }
        
        // Clear choices list and hide panel.
        choices.Clear();
        choicePanel.SetActive(false);
        NextSentence();
    }
}
