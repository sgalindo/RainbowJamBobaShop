using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BobbingKey : MonoBehaviour
{
    public float bobAmplitude = 0.2f;
    public float bobFrequency = 2;
    private Vector3 startPos;
    private TextMeshPro tmp;

    // Start is called before the first frame update
    void Awake()
    {
        startPos = transform.localPosition;
        tmp = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, startPos.y + (Mathf.Sin(Time.fixedTime * Mathf.PI * bobFrequency) * bobAmplitude), transform.localPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            tmp.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            tmp.enabled = false;
    }
}
