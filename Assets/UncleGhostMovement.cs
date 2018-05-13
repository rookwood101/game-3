using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncleGhostMovement : MonoBehaviour {

    private Pathfinding.AIDestinationSetter uncleGhostTarget;
    private GameObject naughtyBoy;

    // Use this for initialization
    void Start () {
        naughtyBoy = GameObject.Find("Ghost");
        uncleGhostTarget = GetComponent<Pathfinding.AIDestinationSetter>();
        uncleGhostTarget.target = naughtyBoy.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
