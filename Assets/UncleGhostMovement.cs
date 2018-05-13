using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncleGhostMovement : MonoBehaviour {

    private Pathfinding.AIDestinationSetter uncleGhostTarget;
    private IAstarAI ai;
    private GameObject naughtyBoy;

    // Use this for initialization
    void Start () {
        ai = GetComponent<IAstarAI>();
        naughtyBoy = GameObject.Find("Ghost");
        uncleGhostTarget = GetComponent<Pathfinding.AIDestinationSetter>();
        uncleGhostTarget.target = naughtyBoy.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (ai.velocity.x < 0)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        } else if (ai.velocity.x > 0)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }
    }


}
