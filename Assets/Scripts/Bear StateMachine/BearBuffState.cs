using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BearBuffState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Play growl or buff sound
        //SoundManager.Instance.Play("GrowlSound");
        // Additional buff effects or logic
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Switch back to attacking after the buff animation ends
        animator.SetBool("isAttacking", true);
    }
}

