using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour {

    public LayerMask ghostLayer;
    public float ghostRadius;
    public GameObject ghost;
    public GameObject target;

    private bool isNearGhost;
    private bool isRunningAway;
    private float speed = 700f;
    private Rigidbody2D rb;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        EventManager.AddListener(EventTypes.Runaway, Runaway);
	}
	
	// Update is called once per frame
	void Update () {

        Runaway(ghost);
        UpdateRun();

	}

    private void UpdateRun()
    {
        if (!(isNearGhost = Physics2D.OverlapCircle(transform.position, ghostRadius, ghostLayer)) && isRunningAway)
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            isRunningAway = false;
        }
        else if (isRunningAway)
        {
            // Get direction to move away from ghost
            Vector3 direction = transform.position - ghost.transform.position;
            direction.z = 0;

            Vector3 newDirection = direction.normalized;
            

            target.transform.position = transform.position + newDirection * 40;

            GetComponent<Pathfinding.AIDestinationSetter>().target = target.transform;
        }
    }

    private void Runaway(object chaser)
    {
        if(isNearGhost && ((GameObject)chaser).name == "Ghost")
        {
            isRunningAway = true;
        }
    }


}
