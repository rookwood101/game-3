using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthEnbiggener : MonoBehaviour {
    private float scale = 0.1f;
    private bool hasStarted;

    private void Awake()
    {
        EventManager.AddListener(EventTypes.Start, MouthBig);
    }
    void Update()
    {
        if (!hasStarted || scale > 100)
            return;

        scale *= 1.04f;
        transform.localScale = new Vector3(0.5f + scale, 0.5f + scale, 1);

    }

    private void MouthBig(object _)
    {
        if (hasStarted)
            return;

        hasStarted = true;
    }
}
