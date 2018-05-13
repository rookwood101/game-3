using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    private Vector3 offset;
    private ColorCurvesManager ccm;

    private void Awake()
    {
        ccm = GetComponent<ColorCurvesManager>();
        offset = transform.position - player.transform.position;
        EventManager.AddListener(EventTypes.Spookometer, OnSpookometer);
    }

    private void OnSpookometer(object arg0)
    {
        float spookometer = (float)arg0;
        ccm.Factor = Mathf.Max(0, spookometer - 1.3f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
