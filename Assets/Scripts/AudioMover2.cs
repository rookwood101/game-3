using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pitch;
using System;

public class AudioMover2 : MonoBehaviour
{
    private const float MID_PITCH = 6f;

    private Rigidbody2D rb;

    private void Awake()
    {
        EventManager.AddListener(EventType.MicrophonePitch, OnMicrophonePitch);
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMicrophonePitch(object pitchObj)
    {
        float? pitch = (float?)pitchObj;

        if (pitch.HasValue)
        {
            Vector2 force = new Vector2(0, (pitch.Value - MID_PITCH) * speed);
            rb.AddForce(force);
        }
    }
}
