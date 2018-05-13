using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private bool isWandering;

    private Rigidbody2D rb;

    public int investigateSpeed;
    public int runSpeed;
    public int wanderSpeed;


    private void Awake()
    {
        target = new GameObject("Target");
        rb = GetComponent<Rigidbody2D>();
        NPCPath = GetComponent<Pathfinding.AIPath>();
        NPCDestination = GetComponent<Pathfinding.AIDestinationSetter>();
        ghost = GameObject.Find("Ghost");

        EventManager.AddListener(EventTypes.Runaway, Runaway);
        EventManager.AddListener(EventTypes.Investigate, Investigate);
        EventManager.AddListener(EventTypes.StopRunaway, RunToDoor);
    }

    // Use this for initialization
    void Start()
    {
        MovementHandling();

    }

    private void RunToDoor(object npcRunning)
    {
        if ((GameObject)npcRunning == gameObject)
        {
            isRunningAway = false;
            NPCDestination.target = GameObject.Find("FrontDoor").transform;
        }
    }

    // Update is called once per frame
    private async Task MovementHandling()
    {
        while (true)
        {
            if (isRunningAway)
            {
                Debug.Log("Running");
                UpdateRun();
            }
            else if (isInvestigating)
            {
                Debug.Log("investigation");

                UpdateInvestigate();
            }
            else
            {
                Debug.Log("wandering");

                await UpdateWandering();
            }
            await Wait.ForIEnumerator(null);
        }
    }

    private async Task UpdateWandering()
    {
        // If NPC reaches end of path and there is no new path set or start of game
        if (!NPCPath.pathPending && NPCPath.reachedEndOfPath || NPCDestination.target == null)
        {
            // Pause
            NPCPath.canMove = false;
            NPCPath.maxSpeed = wanderSpeed;

            // Move NPC to random point in radius around NPC
            target.transform.position = PickRandomPoint();

            NPCDestination.target = target.transform;

            //After 7 seconds start following the path
            await Wait.ForIEnumerator(new WaitForSeconds(7));
            NPCPath.canMove = true;
        }

    }

    private Vector2 PickRandomPoint()
    {
        // Pick a random point in an area
        Vector3 point = Random.insideUnitCircle * wanderingRadius;
        point.z = 0;

        // Set are to around NPC
        point += NPCPath.position;
        return point;

    }

    private void UpdateInvestigate()
    {
        // Move slowly towards ghost if speaking quietly
        NPCPath.maxSpeed = investigateSpeed;
        NPCPath.canMove = true;

        NPCDestination.target = ghost.transform;
    }

    private void UpdateRun()
    {
        // Run fast away from ghost if speaking loudly
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
            ghost.layer = 8;

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
