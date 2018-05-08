using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pitch;
using System;

public class AudioMover2 : MonoBehaviour
{
    private float midPitch = 6.8f;
    private float speed = 10f;
    private Rigidbody2D rb;

    private void Awake()
    {
        EventManager.AddListener(EventType.MicrophonePitch, OnMicrophonePitch);
        EventManager.AddListener(EventType.NewPitchBounds, OnNewPitchBounds);
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnNewPitchBounds(object pitchBoundsObj)
    {
        float[] pitchBounds = (float[])pitchBoundsObj;
        midPitch = (pitchBounds[0] + pitchBounds[1]) / 2;
    }

    private void OnMicrophonePitch(object pitchObj)
    {
        float? pitch = (float?)pitchObj;

        if (pitch.HasValue)
        {
            Vector2 force = new Vector2(0, (pitch.Value - midPitch) * speed);
            rb.AddForce(force);
        }
    }
}
