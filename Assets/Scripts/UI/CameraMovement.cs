using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private PlayerMovement player;
    public Transform maxPos;
    public Transform minPos;
    private Camera cam;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {

        if (player.transform.position.y > (transform.position.y + ((cam.aspect / 4) * cam.orthographicSize) - (player.GetComponent<SpriteRenderer>().bounds.size.y / 2))
            && transform.position.y < maxPos.position.y)
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, player.transform.position.y, 0.06f), -10);

        if (player.transform.position.y < (transform.position.y - ((cam.aspect / 2) * cam.orthographicSize) + (player.GetComponent<SpriteRenderer>().bounds.size.y / 2))
            && transform.position.y > minPos.position.y)
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, player.transform.position.y, 0.02f), -10);

        if (transform.position.y > maxPos.position.y)
            transform.position = new Vector3(transform.position.x, maxPos.position.y, -10);

        if (transform.position.y < minPos.position.y)
            transform.position = new Vector3(transform.position.x, minPos.position.y, -10);
    }
}
