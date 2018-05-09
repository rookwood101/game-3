using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Spookometer : MonoBehaviour
{
    float minPitch = 0;
    float maxPitch = 0;
    float _pitchSampleRange = 3; // in seconds
    int pitchSampleCountBytes;
    float compressionRatio = 0;
    private Text text;

    List<byte> pitchSamples = new List<byte>();

    private void Awake()
    {
        text = GetComponent<Text>();
        pitchSampleCountBytes = (int)(_pitchSampleRange / Time.fixedDeltaTime) * 4;
        EventManager.AddListener(EventTypes.MicrophonePitch, OnMicrophonePitch);
        EventManager.AddListener(EventTypes.NewPitchBounds, OnNewPitchBounds);
    }

    private void OnNewPitchBounds(object pitchBoundsObj)
    {
        float[] pitchBounds = (float[])pitchBoundsObj;
        minPitch = pitchBounds[0];
        maxPitch = pitchBounds[1];
    }

    private void OnMicrophonePitch(object pitchObj)
    {
        float? pitch = (float?)pitchObj;
        if (pitch.HasValue)
        {
            pitchSamples.AddRange(BitConverter.GetBytes(pitch.Value));
        }
        else
        {
            pitchSamples.AddRange(BitConverter.GetBytes(0f));
        }

        if (pitchSamples.Count >= pitchSampleCountBytes)
        {
            byte[] pitchRecentSamples = new byte[pitchSampleCountBytes];
            pitchSamples.CopyTo(pitchSamples.Count - pitchSampleCountBytes, pitchRecentSamples, 0, pitchSampleCountBytes);
            byte[] compressedSamples = CLZF2.Compress(pitchRecentSamples);
            compressionRatio = (float)compressedSamples.Length / (float)pitchSampleCountBytes;
            text.text = compressionRatio.ToString();
        }
    }
}
