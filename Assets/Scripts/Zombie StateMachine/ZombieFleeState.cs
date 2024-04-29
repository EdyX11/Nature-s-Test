using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieFleeState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform zombieTransform;
    Vector3 fleeDirection;
    public float fleeSpeed = 5.0f; // Speed at which the zombie will flee

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     
        agent = animator.GetComponent<NavMeshAgent>();
        zombieTransform = animator.transform;

      
        if (agent != null)
        {
            agent.enabled = true;

            
            fleeDirection = -zombieTransform.forward;

            // Rotate the zombie 180 degrees around the Y axis
            zombieTransform.Rotate(0f, 180f, 0f);

            agent.speed = fleeSpeed;
            agent.SetDestination(zombieTransform.position + fleeDirection * 10); // Flee to a point 10 units behind
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      
        if (agent != null && agent.enabled)
        {
            agent.SetDestination(zombieTransform.position + fleeDirection * 10);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (agent != null)
        {
            agent.speed = 3.5f; // Reset to default walking speed or another appropriate value
        }
    }
}
