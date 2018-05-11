﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGameObject : MonoBehaviour
{
    public GameObject toTrack;
    public Vector3 offset;


    private void LateUpdate()
    {
        transform.position = toTrack.transform.position + offset;
    }
}
