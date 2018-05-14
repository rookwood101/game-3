using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostVibration : MonoBehaviour {
    private float vibrationSpeed = 10f;
    private Vector3 previousMove = Vector3.zero;
    private bool hasStarted;

    private void Awake()
    {
        EventManager.AddListener(EventTypes.Start, Vibrate);
    }

    private void Update()
    {
        if (!hasStarted || vibrationSpeed > 100)
            return;

        vibrationSpeed *= 1.04f;
        Vector3 newMove = new Vector3(10 * 0.003f * Mathf.Sin(Time.time * vibrationSpeed), 0, 0);
        transform.localPosition = transform.localPosition - previousMove + newMove;
        previousMove = newMove;
    }

    private void Vibrate(object _)
    {
        if (hasStarted)
            return;

        hasStarted = true;
    }
}
