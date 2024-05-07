using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearAttackState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;

    public float stopAttackingDistance = 3f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //initialization
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        
        if (BigBearEnemy.Instance.bearChannel.isPlaying == false)
        {

            BigBearEnemy.Instance.bearChannel.PlayOneShot(BigBearEnemy.Instance.bearAttack);

        }
        LookAtPlayer();

        // check if agent should stop attacking

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        //check if agent should stop chasing

        if (distanceFromPlayer > stopAttackingDistance)

        {
            animator.SetBool("isAttacking", false);

        }

        // Stop attacking if health drops below threshold

      /*  if (BigBearEnemy.Instance.HP != 0 && BigBearEnemy.Instance.HP < 40)
        {
            animator.SetBool("isAttacking", false);
            animator.SetTrigger("BearBuff");
            Debug.Log("Bear Buff Triggered");
        }
      */

    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //  BigBearEnemy.Instance.bearChannel.Stop();

    }






    private void LookAtPlayer() // to face the player 
    {

        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);


    }
}
