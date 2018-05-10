using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmISpooked : MonoBehaviour
{
    private float spookometer = 0;
    private GameObject ghost;// inverse square law - spooked less if ghost further away
    private float volume = 0;
    private const float QUIET_THRESHOLD = 0.25f;
    private const float LOUD_THRESHOLD = 0.75f;

    [HideInInspector]
    public float tenseness;// How tense they are right now - quiet spookyness brings this up
    [SerializeField]
    private float confidence;// Their general confidence - affects how tense they get

    [HideInInspector]
    public float fear = 0; // how scared they are right now - loud spookiness brings this up * by tenseness
    [SerializeField]
    private float indifference;// Their general indifference - affects how fearful they get

    private void Awake()
    {
        ghost = GameObject.Find("Ghost");
        EventManager.AddListener(EventTypes.Spookometer, OnSpookometerChange);
        EventManager.AddListener(EventTypes.MicrophoneVolume, OnMicrophoneVolumeChange);
    }

    private void Update()
    {
        if (spookometer > 1 && (transform.position - ghost.transform.position).magnitude < 5)
        {
            fear += (spookometer - 1) * Time.deltaTime;
        }
        if (fear > 6)
        {
            EventManager.TriggerEvent(EventTypes.Runaway, gameObject);
        }
    }

    private void UpdateFancy()
    {
        // float ghostInverseSquareDistance = 1f / (transform.position - ghostPosition).sqrMagnitude;
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
