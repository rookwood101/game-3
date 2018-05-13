using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHover : MonoBehaviour
{
    Vector3 previousMove = Vector3.zero;
    private float minPitch = 6.5f;
    private float maxPitch = 9f;

    private float vibrationSpeed = 0;
    private readonly float maxVibration = 50;

    private void Awake()
    {
        EventManager.AddListener(EventTypes.MicrophonePitch, OnMicrophonePitch);
    }

    private void OnMicrophonePitch(object pitchObj)
    {
        float? pitch = (float?)pitchObj;

        if (pitch.HasValue)
        {
            vibrationSpeed = (pitch.Value - minPitch) / (maxPitch - minPitch) * maxVibration;
        }
        else
        {
            vibrationSpeed = 0;
        }
    }
    private void Update()
    {
        Vector3 newMove = new Vector3(vibrationSpeed * 0.003f * Mathf.Sin(Time.time * vibrationSpeed), 0.2f * Mathf.Sin(Time.time * 4f), 0);
        transform.localPosition = transform.localPosition - previousMove + newMove;
        previousMove = newMove;
    }
}
