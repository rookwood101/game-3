using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNPCs : MonoBehaviour {

    public GameObject NPCPrefab;

    public float spawnTime;
    public int maxNPCs;
    private float timeLeft;
    private int counter;

    private void Awake()
    {
        counter = 1;
        EventManager.AddListener(EventTypes.NPCExit, DecrementCounter);
    }

    private void DecrementCounter(object _)
    {
        counter--;
    }

    // Use this for initialization
    void Start () {
        timeLeft = spawnTime;
	}
	
	// Update is called once per frame
	void Update () {
        // Countdown
        if(counter > maxNPCs)
        {
            return;
        }

        timeLeft -= Time.deltaTime;
        if(timeLeft < 0)
        {
            // Spawn and reset countdown
            Instantiate(NPCPrefab, GameObject.Find("FrontDoor").transform.position - new Vector3(1,0,0), Quaternion.identity);
            timeLeft = spawnTime;
            counter++;
        }
	}
}
