using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNPCs : MonoBehaviour {

    public GameObject NPCPrefab;

    public float spawnTime;
    private float timeLeft;

	// Use this for initialization
	void Start () {
        timeLeft = spawnTime;
	}
	
	// Update is called once per frame
	void Update () {
        // Countdown
        timeLeft -= Time.deltaTime;
        if(timeLeft < 0)
        {
            // Spawn and reset countdown
            Instantiate(NPCPrefab, GameObject.Find("FrontDoor").transform.position - new Vector3(1,0,0), Quaternion.identity);
            timeLeft = spawnTime;
        }
	}
}
