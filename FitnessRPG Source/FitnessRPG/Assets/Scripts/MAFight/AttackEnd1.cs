using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnd1 : StateMachineBehaviour
{
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack", false);
    }
}
