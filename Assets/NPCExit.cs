using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCExit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Check if player leaves
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject touchingObject = collision.gameObject;

        //Check if NPC is touching the door and npc is trying to leave
        if(touchingObject.tag == "NPC" && touchingObject.GetComponent<Pathfinding.AIDestinationSetter>().target == gameObject.transform)
        {
            EventManager.TriggerEvent(EventTypes.NPCExit, touchingObject);
        }
    }
}
