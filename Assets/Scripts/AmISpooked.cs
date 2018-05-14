using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmISpooked : MonoBehaviour
{
    private float spookometerTotal = 0;
    private uint spookometerSamples = 0;
    private GameObject ghost;// inverse square law - spooked less if ghost further away
    private float volumeTotal = 0;
    private uint volumeSamples = 0;
    private const float LOW_DISTANCE_THRESHOLD = 5;
    public const float TENSENESS_THRESHOLD = 0.1f;
    public const float FEAR_THRESHOLD = 1f;
    private float lowVolumeThreshold = 0.25f * (1f / (4 * Mathf.PI * LOW_DISTANCE_THRESHOLD * LOW_DISTANCE_THRESHOLD));
    private bool runningAway = false;
    private bool investigating = false;
    private bool isRunningToDoor = false;
    private float timeOfLastSpook = 0;

    [HideInInspector]
    public float tenseness = 0;// How tense they are right now - quiet spookyness brings this up
    private float unconfidence = 100;// Their general confidence - affects how tense they get

    [HideInInspector]
    public float fear = 0; // how scared they are right now - loud spookiness brings this up * by tenseness
    private float fragility = 100;// Their general indifference - affects how fearful they get
    public bool dead = false;

    private void Awake()
    {
        ghost = GameObject.Find("Ghost");
        EventManager.AddListener(EventTypes.Spookometer, OnSpookometerChange);
        EventManager.AddListener(EventTypes.MicrophoneVolume, OnMicrophoneVolumeChange);
        EventManager.AddListener(EventTypes.NewVolumeBounds, OnVolumeBounds);
        // lowVolumeThreshold = ((VolumeSlider.sliderMin + ((VolumeSlider.sliderMin + VolumeSlider.sliderMax) / 2f)) / 2f) * (1f / (4 * Mathf.PI * LOW_DISTANCE_THRESHOLD * LOW_DISTANCE_THRESHOLD));
    }

    private void Start()
    {
        UpdateFancy();
    }

    private void OnVolumeBounds(object volumeBoundsObj)
    {
        float[] volumeBounds = (float[])volumeBoundsObj;
        float midPoint = (volumeBounds[0] + volumeBounds[1]) / 2f;
        lowVolumeThreshold = ((volumeBounds[0] + midPoint) / 2f) * (1f / (4 * Mathf.PI * LOW_DISTANCE_THRESHOLD * LOW_DISTANCE_THRESHOLD));
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

            DoDecay();

            CheckIfSpooked();

            // can't be spooked if spookometer < 1
            if (spookometer < 1)
            {
                continue;
            }
            DoSpooking(spookometer, volume);
        }
    }

    private void DoSpooking(float spookometer, float volume)
    {
        float inverseSquareDistance = 1f / (4 * Mathf.PI * (transform.position - ghost.transform.position).sqrMagnitude);
        float volumeDistance = volume * inverseSquareDistance;
        if ((transform.position - ghost.transform.position).magnitude < 15)
        {
            timeOfLastSpook = Time.time;
            if (volumeDistance < lowVolumeThreshold) // increase tenseness
            {
                float tensenessIncrease = volumeDistance * spookometer * unconfidence;
                tenseness += tensenessIncrease;
            }
            else
            {
                float fearIncrease = volumeDistance * spookometer * fragility;
                fear += fearIncrease;
            }
        }
    }

    private void CheckIfSpooked()
    {
        if (tenseness >= TENSENESS_THRESHOLD && !runningAway && !investigating && !isRunningToDoor && !dead)
        {
            investigating = true;
            EventManager.TriggerEvent(EventTypes.RunawayAnywhere, gameObject);
        }
        if (tenseness < TENSENESS_THRESHOLD && !runningAway && investigating && !isRunningToDoor && !dead)
        {
            investigating = false;
            EventManager.TriggerEvent(EventTypes.StopRunawayAnywhere, gameObject);
        }
        if (fear >= FEAR_THRESHOLD && !runningAway && tenseness >= TENSENESS_THRESHOLD && !isRunningToDoor && !dead)
        {
            isRunningToDoor = true;
            runningAway = true;
            EventManager.TriggerEvent(EventTypes.RunawayToDoorNew, gameObject);
        }
        if (fear >= FEAR_THRESHOLD && tenseness < TENSENESS_THRESHOLD && !runningAway && !isRunningToDoor && !investigating && !dead)
        {
            dead = true;
            EventManager.TriggerEvent(EventTypes.Dead, gameObject);
        }
    }

    private void DoDecay()
    {
        if (Time.time - timeOfLastSpook > 3)
        {
            if (tenseness > 0.01f)
            {
                tenseness -= 0.01f;
            }
            else
            {
                tenseness = 0;
            }
            if (fear > 0.1f)
            {
                fear -= 0.1f;
            }
            else
            {
                fear = 0;
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
