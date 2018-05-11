using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmISpooked : MonoBehaviour
{
    private float spookometer = 0;
    private GameObject ghost;// inverse square law - spooked less if ghost further away
    private float volume = 0;
    private float lowVolumeThreshold = 0.25f;
    private float highVolumeThreshold = 0.75f;
    private float lowDistanceThreshold = 7;

    [HideInInspector]
    public float tenseness;// How tense they are right now - quiet spookyness brings this up
    [SerializeField]
    private float unconfidence;// Their general confidence - affects how tense they get

    [HideInInspector]
    public float fear = 0; // how scared they are right now - loud spookiness brings this up * by tenseness
    [SerializeField]
    private float fragility;// Their general indifference - affects how fearful they get

    private void Awake()
    {
        ghost = GameObject.Find("Ghost");
        EventManager.AddListener(EventTypes.Spookometer, OnSpookometerChange);
        EventManager.AddListener(EventTypes.MicrophoneVolume, OnMicrophoneVolumeChange);
        EventManager.AddListener(EventTypes.NewVolumeBounds, OnVolumeBounds);
    }

    private void OnVolumeBounds(object volumeBoundsObj)
    {
        float[] volumeBounds = (float[])volumeBoundsObj;
        float midPoint = (volumeBounds[0] + volumeBounds[1]) / 2f;
        lowVolumeThreshold = ((volumeBounds[0] + midPoint) / 2f) * (1f / 4 * Mathf.PI * lowDistanceThreshold * lowDistanceThreshold);
    }

    private void Update()
    {
        UpdateFancy();
        // if (spookometer > 1 && (transform.position - ghost.transform.position).magnitude < 5)
        // {
        //     fear += (spookometer - 1) * Time.deltaTime;
        // }
        // if (fear > 3)
        // {
        //     EventManager.TriggerEvent(EventTypes.Runaway, gameObject);
        // }
    }

    private void UpdateFancy()
    {
        if (spookometer < 1)
        {
            return;
        }
        float inverseSquareDistance = 1f / 4 * Mathf.PI * (transform.position - ghost.transform.position).sqrMagnitude;
        float volumeDistance = volume * inverseSquareDistance;
        if (volumeDistance < lowVolumeThreshold) // increase tenseness
        {
            tenseness += volumeDistance * spookometer * unconfidence;
        }
        else
        {
            fear += volumeDistance * spookometer * fragility;
        }
    }

    private void OnMicrophoneVolumeChange(object volumeObj)
    {
        volume = (float)volumeObj;
    }

    private void OnSpookometerChange(object spookometerObj)
    {
        spookometer = (float)spookometerObj;
    }
}
