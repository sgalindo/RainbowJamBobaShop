using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CloseShop : MonoBehaviour
{
    GameManager gm;
    public TextMeshPro tmp;
    private bool closing = false;
    private bool confirmed = false;
    private EventSystem es;

    public Image confirmImage;
    public Button noButton;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        es = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (!confirmed && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!closing)
            {
                confirmImage.gameObject.SetActive(true);
                es.SetSelectedGameObject(noButton.gameObject);
                closing = true;
            }
        }


        if (gm.finishedCount >= 5)
        {
            tmp.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            tmp.enabled = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            tmp.enabled = false;
        }

    }

    public void DisableMenu()
    {
        closing = false;
        confirmImage.gameObject.SetActive(false);
    }

    public void EndGame()
    {
        confirmed = true;
        DisableMenu();
    }
}
