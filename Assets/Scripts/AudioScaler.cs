using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScaler : MonoBehaviour
{
    private float minVolume = 0;
    private float maxVolume = 1;
    private void Awake()
    {
        EventManager.AddListener(EventTypes.MicrophoneVolume, OnMicrophoneVolume);
        EventManager.AddListener(EventTypes.NewVolumeBounds, OnNewVolumeBounds);
    }

    private void OnNewVolumeBounds(object volumeBoundsObj)
    {
        float[] volumeBounds = (float[])volumeBoundsObj;
        minVolume = volumeBounds[0];
        maxVolume = volumeBounds[1];
    }

    private void OnMicrophoneVolume(object volumeObj)
    {
        float volume = (float)volumeObj;
        volume = Mathf.Min(maxVolume, volume);
        volume = Mathf.Max(volume, minVolume);

        float normalisedVolume = (volume - minVolume) / (maxVolume - minVolume);
        transform.localScale = new Vector3(0.5f + normalisedVolume, 0.5f + normalisedVolume, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
