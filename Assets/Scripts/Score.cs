using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

    private int score;
    private bool isInPerson;

    private void Awake()
    {
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
        EventManager.TriggerEvent(EventTypes.UpdateScore, score.ToString());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInPerson = false;

    }

}
