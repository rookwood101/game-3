using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNPCs : MonoBehaviour
{

    public GameObject NPCPrefab;
    public GameObject UncleGhostPrefab;

    public float spawnTime;
    public int maxNpcs;
    public int npcsUntilUncleGhost;
    private float timeLeft;
    private int counter;
    private int numberOfNPCsScared;

    private void Awake()
    {
        counter = 1;
        numberOfNPCsScared = 0;
        EventManager.AddListener(EventTypes.NPCExit, DecrementCounter);
        EventManager.AddListener(EventTypes.Dead, OnDeath);
    }

    private void OnDeath(object arg0)
    {
        counter--;
        numberOfNPCsScared++;
    }

    private void DecrementCounter(object _)
    {
        numberOfNPCsScared++;
        counter--;
    }

    // Use this for initialization
    void Start()
    {
        timeLeft = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfNPCsScared > npcsUntilUncleGhost)
        {
            Instantiate(UncleGhostPrefab, GameObject.Find("FrontDoor").transform.position - new Vector3(1, 0, 0), Quaternion.identity);
            numberOfNPCsScared = 0;
        }
        // Limit number of NPCs
        if (counter > maxNpcs)
        {
            return;
        }
        // Countdown
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            // Spawn and reset countdown
            Instantiate(NPCPrefab, GameObject.Find("FrontDoor").transform.position - new Vector3(1, 0, 0), Quaternion.identity);
            timeLeft = spawnTime;
            counter++;
        }
    }
}
