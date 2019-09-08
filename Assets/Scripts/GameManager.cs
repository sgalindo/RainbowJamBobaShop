using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public NPC[] npcs;
    private int index = 0;
    public int finishedCount = 0;
    private bool tutorialFinished = false;
    private bool gameEnded = false;
    public DialogueManager dm;
    public DialogueSystem ds;
    public Animator anim;

    public TextAsset tutorialIntro;

    // Start is called before the first frame update
    void Start()
    {
        ds.CreateStory(tutorialIntro.text);
        dm.InitializeDialogue(ds);
        dm.endDialogueEvent += FinishTutorial;
        PlayerInteract.interacting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!tutorialFinished)
        {
            if (Input.GetButtonDown("Interact"))
                dm.NextSentence();
        }
        else
        {
            if (finishedCount < npcs.Length)
            {
                NextNPC();
            }
        }

        if (gameEnded)
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

    }

    private void NextNPC()
    {
        if (!npcs[index].gameObject.activeSelf) npcs[index].gameObject.SetActive(true);
    }

    public void FinishNPC()
    {
        finishedCount++;
        index++;
    }

    public void EndGame()
    {
        anim.SetTrigger("End");
        gameEnded = true;
    }

    private void FinishTutorial(bool enabled)
    {
        tutorialFinished = true;
        dm.endDialogueEvent -= FinishTutorial;
        PlayerInteract.interacting = false;
        NextNPC();
    }
}
