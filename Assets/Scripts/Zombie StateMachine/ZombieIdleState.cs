using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{
    float timer;
    public float idleTime = 0f;

    Transform player;


    public float detectionAreaRadius = 18f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //transition to patrol state

        timer += Time.deltaTime;

        if(timer > idleTime)
        {

            animator.SetBool("isPatroling", true);

        }

        //transition to chase state if player inside

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);//calc distance between player and enemy

        if(distanceFromPlayer < detectionAreaRadius) // if smaller , player is inside
        {

            animator.SetBool("isChasing", true);
        }
    }



}
