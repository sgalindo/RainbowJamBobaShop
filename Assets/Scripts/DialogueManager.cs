using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

// This script will handle dialogue display and choices.
// Basically interprets a DialogueSystem and sets the UI text accordingly.
public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject choicePanel;
    public GameObject buttonPrefab;
    private TextMeshProUGUI displayText;
    private DialogueSystem currentSystem;
    private Story story;
    private List<Button> choices;

    public DialogueSystem dialogue; // ONLY FOR TESTING

    private void Awake()
    {
        displayText = dialoguePanel.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        dialoguePanel.SetActive(false);
        choices = new List<Button>();
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

    // Continue the story. Either gets the next available sentence, 
    // displays choices,
    // or closes the dialogue at the end of the story.
    private void NextSentence()
    {
        if (story.canContinue)
        {
            displayText.text = "";
            StopAllCoroutines();
            StartCoroutine(Type(story.Continue()));
        }
        else if (story.currentChoices.Count > 0 && choices.Count == 0)
            PopulateChoices();
        else
            CloseDialogue();
    }

    // Types out the current sentence letter by letter at the currentSystem's speed
    private IEnumerator Type(string sentence)
    {
        foreach (char letter in sentence.ToCharArray())
        {
            displayText.text += letter;
            yield return new WaitForSeconds(currentSystem.speed);
        }
    }

    private void CloseDialogue()
    {
        StopAllCoroutines();
        displayText.text = "";
        displayText.enabled = false;
        currentSystem = null;
    }

    // Instantiates buttons for each available choice in the current point in the story
    private void PopulateChoices()
    {
        for (int i = 0; i < story.currentChoices.Count; i++)
        {
            GameObject btn = Instantiate(buttonPrefab, choicePanel.transform);
            btn.GetComponent<ChoiceButton>().index = i;
            btn.transform.Find("ChoiceText").GetComponent<TextMeshProUGUI>().text = story.currentChoices[i].text;
            btn.GetComponent<Button>().onClick.AddListener(SelectButton);

            choices.Add(btn.GetComponent<Button>());
        }
    }

    private void SelectButton()
    {
        // TODO: Handle choice button-specific choice selection 
    }
}
