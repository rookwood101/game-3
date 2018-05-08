using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pitch;
using System;

public class AudioMover2 : MonoBehaviour
{
    private float speed = 10;
    private const float MID_PITCH = 6f;

    private Rigidbody2D rb;

    private void Awake()
    {
        EventManager.AddListener(EventType.MicrophonePitch, OnMicrophonePitch);
    }

    private void OnMicrophonePitch(object pitchObj)
    {
        float? pitch = (float?)pitchObj;

        if (pitch.HasValue)
        {
            Debug.Log("adding force");
            rb.AddForce(new Vector2(0, (pitch.Value - MID_PITCH) * speed));
        }
    }
}
