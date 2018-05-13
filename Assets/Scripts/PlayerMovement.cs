﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.position = transform.position + new Vector3(x, y, 0);

    }
}
