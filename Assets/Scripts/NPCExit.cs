using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCExit : MonoBehaviour
{

    private bool exited;
    private AudioSource[] shouts;

    // Use this for initialization
    void Start()
    {
        shouts = GetComponents<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Check if player leaves
    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (exited)
            return;

        GameObject touchingObject = collision.gameObject;

        //Check if NPC is touching the door and npc is trying to leave
        if (touchingObject.name == "FrontDoor" && GetComponent<Pathfinding.AIDestinationSetter>().target == touchingObject.transform)
        {
            exited = true;
            shouts[3].Play();
            await Wait.ForIEnumerator(new WaitForSeconds(0.5f));
            EventManager.TriggerEvent(EventTypes.NPCExit, gameObject);

        }
    }
}
