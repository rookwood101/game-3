using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHover : MonoBehaviour
{
    Vector3 previousMove = Vector3.zero;
    private void Update()
    {
        Vector3 newMove = new Vector3(0, 0.2f * Mathf.Sin(Time.time * 4f), 0);
        transform.localPosition = transform.localPosition - previousMove + newMove;
        previousMove = newMove;
    }
}
