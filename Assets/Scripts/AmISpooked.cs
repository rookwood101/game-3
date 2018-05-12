using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AmISpooked : MonoBehaviour
{
    private float spookometerTotal = 0;
    private uint spookometerSamples = 0;
    private GameObject ghost;// inverse square law - spooked less if ghost further away
    private float volumeTotal = 0;
    private uint volumeSamples = 0;
    private float lowVolumeThreshold = 0.25f;
    private float highVolumeThreshold = 0.75f;
    private float lowDistanceThreshold = 10;
    private bool runningAway = false;

    [HideInInspector]
    public float tenseness = 0;// How tense they are right now - quiet spookyness brings this up
    private float unconfidence = 100;// Their general confidence - affects how tense they get

    [HideInInspector]
    public float fear = 0; // how scared they are right now - loud spookiness brings this up * by tenseness
    private float fragility = 100;// Their general indifference - affects how fearful they get

    private void Awake()
    {
        ghost = GameObject.Find("Ghost");
        EventManager.AddListener(EventTypes.Spookometer, OnSpookometerChange);
        EventManager.AddListener(EventTypes.MicrophoneVolume, OnMicrophoneVolumeChange);
        EventManager.AddListener(EventTypes.NewVolumeBounds, OnVolumeBounds);
    }

    private void Start()
    {
        UpdateFancy();
    }

    private void OnVolumeBounds(object volumeBoundsObj)
    {
        float[] volumeBounds = (float[])volumeBoundsObj;
        float midPoint = (volumeBounds[0] + volumeBounds[1]) / 2f;
        lowVolumeThreshold = ((volumeBounds[0] + midPoint) / 2f) * (1f / (4 * Mathf.PI * lowDistanceThreshold * lowDistanceThreshold));
    }

    private async Task UpdateFancy()
    {
        while (true)
        {
            await Wait.ForIEnumerator(new WaitForSeconds(0.2f));
            if (spookometerSamples == 0 || volumeSamples == 0)
            {
                continue;
            }
            float spookometer = spookometerTotal / spookometerSamples;
            spookometerTotal = 0;
            spookometerSamples = 0;
            float volume = volumeTotal / volumeSamples;
            volumeTotal = 0;
            volumeSamples = 0;

            // decay
            if (tenseness > 0.01f)
            {
                tenseness -= 0.01f;
            }
            else
            {
                tenseness = 0;
            }
            if (fear > 0.01f)
            {
                fear -= 0.01f;
            }
            else
            {
                fear = 0;
            }

            if (fear > 5 && !runningAway)
            {
                EventManager.TriggerEvent(EventTypes.Runaway, gameObject);
            }
            if (fear < 5 && runningAway)
            {
                EventManager.TriggerEvent(EventTypes.StopRunaway, gameObject);
            }
            if (spookometer < 1)
            {
                continue;
            }
            float inverseSquareDistance = 1f / (4 * Mathf.PI * (transform.position - ghost.transform.position).sqrMagnitude);
            float volumeDistance = volume * inverseSquareDistance;
            if (volumeDistance < lowVolumeThreshold) // increase tenseness
            {
                float tensenessIncrease = volumeDistance * spookometer * unconfidence;
                tenseness += tensenessIncrease;
            }
            else
            {
                float fearIncrease = volumeDistance * spookometer * fragility * Mathf.Log(tenseness + 1);
                fear += fearIncrease;
            }
        }
    }

    private void OnMicrophoneVolumeChange(object volumeObj)
    {
        volumeTotal += (float)volumeObj;
        volumeSamples++;
    }

    private void OnSpookometerChange(object spookometerObj)
    {
        spookometerTotal += (float)spookometerObj;
        spookometerSamples++;
    }
}
