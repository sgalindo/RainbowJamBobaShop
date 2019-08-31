using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to ChoiceButton prefab.
public class ChoiceButton : MonoBehaviour
{
    public int index; // Choice index in its current story (set by DialogueManager when instantiated)

    // Event for DialogueManager to subscribe to when this button is selected
    public delegate void ButtonAction(int index);
    public event ButtonAction buttonAction = delegate { };

    // Calls event with its choice index.
    // Called by OnClick() listener when button is selected (in inspector)
    public void SelectButton()
    {
        buttonAction(index);
    }
}
