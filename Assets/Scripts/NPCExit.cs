using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCExit : MonoBehaviour {

    private bool exited;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Check if player leaves
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (exited)
            return;

        GameObject touchingObject = collision.gameObject;

        //Check if NPC is touching the door and npc is trying to leave
        if(touchingObject.name == "FrontDoor" && GetComponent<Pathfinding.AIDestinationSetter>().target == touchingObject.transform)
        {
            exited = true;
            EventManager.TriggerEvent(EventTypes.NPCExit, gameObject);

        }
    }
}
