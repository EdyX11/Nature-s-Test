using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BearPatrolingState : StateMachineBehaviour
{
    float timer;

    public float patrolingTime = 15f;
    Transform player;
    UnityEngine.AI.NavMeshAgent agent;
    public float detectionAreaRadius = 25f;
    public float patrolSpeed = 1f;

    List<Transform> waypointsList = new List<Transform>();


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // initialization
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();

        agent.speed = patrolSpeed;
        timer = 0;

        // move to 1st waypoint

        GameObject waypointCluster = GameObject.FindGameObjectWithTag("WaypointsBear");

        foreach (Transform t in waypointCluster.transform)
        {

            waypointsList.Add(t);

        }

        Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
        agent.SetDestination(nextPosition);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /* if (BigBearEnemy.Instance.bearChannel.isPlaying == false)
         {

             BigBearEnemy.Instance.bearChannel.clip = BigBearEnemy.Instance.zombieWalking;
             BigBearEnemy.Instance.bearChannel.PlayDelayed(1f);
         }
        */
        // check if agent arrived at waypoint and move to next one

        if (agent.remainingDistance <= agent.stoppingDistance)
        {

            agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);

        }

        // transition to idle state

        timer += Time.deltaTime;
        if (timer > patrolingTime)

        {

            animator.SetBool("isPatroling", false);

        }
        //transition to chase state if player inside

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);//calc distance between player and enemy

        if (distanceFromPlayer < detectionAreaRadius) // if smaller , player is inside
        {

            animator.SetBool("isChasing", true);
            Debug.Log($"BEAR CHASING , SOUND TO BE FOUND");
        }



    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // stop agent

        agent.SetDestination(agent.transform.position);
        //BiBearEnemy.Instance.bearChannel.Stop();


    }

}