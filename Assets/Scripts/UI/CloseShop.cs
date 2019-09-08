using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CloseShop : MonoBehaviour
{
    GameManager gm;
    TextMeshPro tmp;
    private bool colliding = false;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        tmp = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        if (colliding && Input.GetButtonDown("Interact"))
            gm.EndGame();

        if (gm.finishedCount >=5)
            tmp.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            tmp.enabled = true;
            colliding = true;
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            tmp.enabled = false;
            colliding = false;
        }
            
    }
}
