using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFlee : MonoBehaviour {

    public LayerMask ghostLayer;
    public float ghostRadius;
    public GameObject ghost;

    public LayerMask wallLayer;
    public float wallRadius;

    private bool isNearGhost;
    private bool isRunningAway;
    private float speed = 600f;



	// Use this for initialization
	void Start () {
        EventManager.AddListener(EventTypes.Runaway, Runaway);
	}
	
	// Update is called once per frame
	void Update () {
        Runaway("ted");

        if (!(isNearGhost = Physics2D.OverlapCircle(transform.position, ghostRadius, ghostLayer)) && isRunningAway)
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            isRunningAway = false;
        }

        ContactFilter2D wallLayerFilter = new ContactFilter2D
        {
            layerMask = wallLayer
        };

        Collider2D[] results = new Collider2D[10];
        if(Physics2D.OverlapCircle(transform.position, wallRadius, wallLayerFilter, results) > 0)
        {
            Debug.Log("wall");
        }
	}

    private void Runaway(object name)
    {
        if(isNearGhost)
        {
            isRunningAway = true;
            Debug.Log(isNearGhost);

            // Get direction to move away from ghost
            Vector3 direction = transform.position - ghost.transform.position;
            direction = direction.normalized;
            direction.z = 0;

            // Move
            GetComponent<Rigidbody2D>().velocity = direction * speed * Time.deltaTime;
        }
    }


}
