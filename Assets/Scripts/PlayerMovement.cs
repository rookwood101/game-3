using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed;
    private Transform child;

    private void Awake()
    {
        child = transform.Find("GhostSprite").transform;
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            child.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            child.localScale = new Vector3(1, 1, 1);
        }
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.position = transform.position + new Vector3(x, y, 0);

    }
}
