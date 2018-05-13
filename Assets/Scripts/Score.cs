using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

    private int score;

    private void Awake()
    {
        EventManager.AddListener(EventTypes.Runaway, AddPoints);
        EventManager.AddListener(EventTypes.NPCExit, AddPoints);
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void AddPoints(object npcScared)
    {
        score++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC")
        {
            score--;
        }
    }

}
