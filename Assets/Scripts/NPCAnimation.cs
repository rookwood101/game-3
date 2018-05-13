using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NPCAnimation : MonoBehaviour
{
    IAstarAI ai;
    Animator animator;
    private void Awake()
    {
        ai = GetComponent<IAstarAI>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Vector3 direction = ai.rotation * Vector3.up;
        // animator.SetFloat("xPos", direction.x);
        // animator.SetFloat("yPos", direction.y);
        Vector3 normalisedVelocity = ai.velocity.normalized;
        if (!ai.canMove)
        {
            normalisedVelocity = Vector3.zero;
        }
        animator.SetFloat("xPos", normalisedVelocity.x);
        animator.SetFloat("yPos", normalisedVelocity.y);
    }
}
