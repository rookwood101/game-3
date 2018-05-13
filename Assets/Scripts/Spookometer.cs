using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Spookometer : MonoBehaviour
{
    // float minPitch = 0;
    // float maxPitch = 0;
    float _pitchSampleRange = 3; // in seconds
    int pitchSampleCount;
    float compressionRatio = 0;
    private Text text;

    List<float> pitchSamples = new List<float>();

    private void Awake()
    {
        text = GetComponent<Text>();
        pitchSampleCount = (int)(_pitchSampleRange / Time.fixedDeltaTime);
        EventManager.AddListener(EventTypes.MicrophonePitch, OnMicrophonePitch);
        // EventManager.AddListener(EventTypes.NewPitchBounds, OnNewPitchBounds);
    }

    private void Update()
    {
        if (compressionRatio > 1)
        {
            text.color = Color.green;
        }
        else
        {
            text.color = Color.grey;
        }
    }

    // private void OnNewPitchBounds(object pitchBoundsObj)
    // {
    //     float[] pitchBounds = (float[])pitchBoundsObj;
    //     minPitch = pitchBounds[0];
    //     maxPitch = pitchBounds[1];
    // }

    private void OnMicrophonePitch(object pitchObj)
    {
        float? pitch = (float?)pitchObj;
        if (pitch.HasValue)
        {
            float roundedPitch = (float)Math.Round(pitch.Value, 1, MidpointRounding.AwayFromZero);
            pitchSamples.Add(roundedPitch);
        }
        else
        {
            pitchSamples.Add(0f);
        }

        if (pitchSamples.Count >= pitchSampleCount)
        {
            byte[] pitchRecentSamplesBytes = new byte[pitchSampleCount * 4];
            Buffer.BlockCopy(pitchSamples.ToArray(), pitchSamples.Count * 4 - pitchSampleCount * 4, pitchRecentSamplesBytes, 0, pitchRecentSamplesBytes.Length);

            byte[] compressedSamples = CLZF2.Compress(pitchRecentSamplesBytes);
            compressionRatio = (float)compressedSamples.Length / (float)pitchSampleCount;
            text.text = compressionRatio.ToString();

            EventManager.TriggerEvent(EventTypes.Spookometer, compressionRatio);
        }
    }
}
