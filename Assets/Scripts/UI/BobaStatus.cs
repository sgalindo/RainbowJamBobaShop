using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SUPER JANK but we are out of time lmao
public class BobaStatus : MonoBehaviour
{
    private PlayerInteract player;
    private SpriteRenderer rend;
    public Sprite[] sprites;

    public float bobAmplitude = 0.5f;
    public float bobFrequency = 1f;
    private Vector3 startPos;

    private void Awake()
    {
        player = transform.parent.GetComponentInChildren<PlayerInteract>();
        rend = GetComponent<SpriteRenderer>();
        startPos = transform.localPosition;
    }

    private void Update()
    {
        if (player.heldBoba != null)
        {
            if (rend.enabled == false)
                rend.enabled = true;

            switch (player.heldBoba.state)
            {
                case Boba.State.Empty:
                    rend.sprite = sprites[0];
                    break;
                case Boba.State.Pearls:
                    rend.sprite = sprites[1];
                    break;
                case Boba.State.Ready:
                    rend.sprite = sprites[2];
                    break;
            }
        }
        else if (rend.enabled == true)
            rend.enabled = false;
    }

    private void FixedUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, startPos.y + (Mathf.Sin(Time.fixedTime * Mathf.PI * bobFrequency) * bobAmplitude), transform.localPosition.z);
    }
}
