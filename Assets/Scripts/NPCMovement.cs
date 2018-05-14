using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCMovement : MonoBehaviour
{

    public int secondsToWait;
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
    private bool isRunningToDoor;
    private bool playedYellowSound;
    private bool playedRedSound;
    private bool isDead = false;

    private Rigidbody2D rb;

    public int investigateSpeed;
    public int runSpeed;
    public int wanderSpeed;

    private AudioSource[] shouts;

    private void Awake()
    {
        target = new GameObject("Target");
        rb = GetComponent<Rigidbody2D>();
        NPCPath = GetComponent<Pathfinding.AIPath>();
        NPCDestination = GetComponent<Pathfinding.AIDestinationSetter>();
        ghost = GameObject.Find("Ghost");

        shouts = GetComponents<AudioSource>();

        EventManager.AddListener(EventTypes.RunawayToDoorNew, Runaway);
        EventManager.AddListener(EventTypes.RunawayAnywhere, Investigate);
        //EventManager.AddListener(EventTypes.StopRunaway, RunToDoor);
        EventManager.AddListener(EventTypes.StopRunawayAnywhere, StopInvestigation);
        //EventManager.AddListener(EventTypes.RunToDoor, RunToDoor);
        EventManager.AddListener(EventTypes.Dead, OnDeath);
    }



    // Use this for initialization
    void Start()
    {
        MovementHandling();

    }

    private void RunToDoor()
    {
        //if ((GameObject)npcRunning == gameObject)
        //{
        isRunningAway = false;
        isInvestigating = false;
        isRunningToDoor = true;
        NPCDestination.target = GameObject.Find("FrontDoor").transform;

        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        //}
    }

    private void OnDeath(object npcDead)
    {
        if ((GameObject)npcDead == gameObject)
        {
            isDead = true;
            NPCPath.canMove = false;
            shouts[2].Play();
        }
    }

    private void StopInvestigation(object npcInvestigating)
    {
        if ((GameObject)npcInvestigating == gameObject)
        {
            isInvestigating = false;
        }
    }

    // Update is called once per frame
    private async Task MovementHandling()
    {
        while (!isDead)
        {
            if (isRunningAway && !isRunningToDoor)
            {
                if (!playedRedSound)
                {
                    playedRedSound = true;
                    shouts[1].Play();
                }
                RunToDoor();
            }
            else if (isInvestigating && !isRunningToDoor)
            {
                if (!playedYellowSound)
                {
                    playedYellowSound = true;
                    shouts[0].Play();
                }
                UpdateInvestigate();
            }
            else if (!isRunningToDoor)
            {
                playedYellowSound = false;
                playedRedSound = false;
                await UpdateWandering();
            }
            await Wait.ForIEnumerator(null);
        }
    }

    private async Task UpdateWandering()
    {
        // If NPC reaches end of path and there is no new path set or start of game
        if ((!NPCPath.pathPending && NPCPath.reachedEndOfPath || NPCDestination.target == null) && SceneManager.GetActiveScene().name != "Tutorial")
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            // Pause
            NPCPath.canMove = false;
            NPCPath.maxSpeed = wanderSpeed;

            // Move NPC to random point in radius around NPC
            target.transform.position = PickRandomPoint();

            NPCDestination.target = target.transform;

            //After 7 seconds start following the path
            await Wait.ForIEnumerator(new WaitForSeconds(secondsToWait));
            NPCPath.canMove = true;

        }

    }

    private Vector2 PickRandomPoint()
    {
        // Pick a random point in an area
        Vector3 point = UnityEngine.Random.insideUnitCircle * wanderingRadius;
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

        // Get direction to move away from ghost
        Vector3 direction = transform.position - ghost.transform.position;
        direction.z = 0;

        Vector3 newDirection = direction.normalized;


        //Change target for AI search
        target.transform.position = transform.position + newDirection * 40;

        NPCDestination.target = target.transform;
        gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
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
