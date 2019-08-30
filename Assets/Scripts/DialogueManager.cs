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
    public EventSystem eventSystem;
    private TextMeshProUGUI displayText;
    private DialogueSystem currentSystem;
    private Story story;
    private List<ChoiceButton> choices;
    private bool isTyping;

    public DialogueSystem dialogue; // ONLY FOR TESTING

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

    private void Update()
    {
        // When interact button is pressed, either start dialogue or continue story
        // (SPACEBAR or E)
        if (Input.GetButtonDown("Interact"))
        {
            if (currentSystem == null)
                OpenDialogue(dialogue);
            else
                NextSentence();
        }
    }

    // Enable dialogue box and set references.
    public void OpenDialogue(DialogueSystem dialogue)
    {
        if (currentSystem == null)
        {
            currentSystem = dialogue;
            story = dialogue.story;
            dialoguePanel.SetActive(true);
            displayText.text = "";
            NextSentence();
        }
    }

    // Continue the story. 
    // Either gets the next available sentence, 
    // displays choices,
    // or closes the dialogue at the end of the story.
    private void NextSentence()
    {
        // If currently typing, stop typing and display whole sentence.
        if (isTyping)
        {
            ResetText();
            displayText.text = story.currentText;
            return;
        }

        // canContinue returns true only if it has another sentence in queue
        // (no choices, and not at end of story)
        if (story.canContinue)
        {
            ResetText();
            StartCoroutine(Type(story.Continue()));
        }
        // Else, display choices if there are any.
        else if (story.currentChoices.Count > 0 && choices.Count == 0)
            PopulateChoices();
        // Otherwise, we're at end of story and we can close the dialogue.
        else
            CloseDialogue();
    }

    // Types out the current sentence letter by letter at the currentSystem's speed
    private IEnumerator Type(string sentence)
    {
        isTyping = true;
        foreach (char letter in sentence.ToCharArray())
        {
            displayText.text += letter;
            yield return new WaitForSeconds(currentSystem.speed);
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
        displayText.enabled = false;
        currentSystem = null;
        dialoguePanel.SetActive(false);
    }

    // Instantiates buttons for each available choice in the current point in the story
    private void PopulateChoices()
    {
        choicePanel.SetActive(true);

        for (int i = 0; i < story.currentChoices.Count; i++)
        {
            GameObject btn = Instantiate(buttonPrefab, choicePanel.transform);
            btn.transform.Find("ChoiceText").GetComponent<TextMeshProUGUI>().text = story.currentChoices[i].text;

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

        NextSentence(); // Finally, advance story.
    }
}
