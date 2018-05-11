using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{

    public LayerMask ghostLayer;
    public float ghostRadius;
    private GameObject ghost;
    public GameObject target;

    public float wanderingRadius;

    private Pathfinding.AIPath NPCPath;
    private Pathfinding.AIDestinationSetter NPCDestination;

    private bool isNearGhost;
    private bool isRunningAway;
    private bool isInvestigating;

    private Rigidbody2D rb;

    public int investigateSpeed;
    public int runSpeed;
    public int wanderSpeed;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        NPCPath = GetComponent<Pathfinding.AIPath>();
        NPCDestination = GetComponent<Pathfinding.AIDestinationSetter>();
        ghost = GameObject.Find("Ghost");

        EventManager.AddListener(EventTypes.Runaway, Runaway);
        EventManager.AddListener(EventTypes.Investigate, Investigate);

    }

    // Update is called once per frame
    void Update()
    {
        // Runaway(gameObject);
        //Checks if NPC is no longer near ghost, if not stop running
        if (!(isNearGhost = Physics2D.OverlapCircle(transform.position, ghostRadius, ghostLayer)) && isRunningAway)
        {
            NPCPath.canMove = false;
            isRunningAway = false;
        }
        else if (isRunningAway)
        {
            UpdateRun();
        }
        else if (isNearGhost && isInvestigating)
        {
            UpdateInvestigate();
        }


    }

    private void UpdateWandering()
    {

    }

    private void UpdateInvestigate()
    {
        NPCPath.maxSpeed = investigateSpeed;
        NPCPath.canMove = true;

        NPCDestination.target = ghost.transform;
    }

    private void UpdateRun()
    {
        NPCPath.maxSpeed = runSpeed;
        NPCPath.canMove = true;
        // Get direction to move away from ghost
        Vector3 direction = transform.position - ghost.transform.position;
        direction.z = 0;

        Vector3 newDirection = direction.normalized;


        //Change target for AI search
        target.transform.position = transform.position + newDirection * 40;

        NPCDestination.target = target.transform;
    }

    private void Runaway(object npc)
    {
        if ((GameObject)npc == gameObject)
        {
            isRunningAway = true;
            isInvestigating = false;
        }
    }

    private void Investigate(object npc)
    {
        if ((GameObject)npc == gameObject)
        {
            if (!isRunningAway)
                isInvestigating = true;
        }
    }
}
