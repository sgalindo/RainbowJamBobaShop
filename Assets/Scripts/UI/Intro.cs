using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [TextArea()]
    public string[] chunks;
    public TextMeshProUGUI tmp;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (index < chunks.Length - 1)
            {
                index++;
                tmp.text = chunks[index];
            }
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
            
    }
}
