//#define PD
using UnityEngine;

public class OnDeath : StateMachineBehaviour
{
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
#if (PD)
    //PD-Version
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Dead", false);
        //animator.SetTrigger("End");
    }
#else
    //MA-Version
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Dead", false);
        animator.SetTrigger("End");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("called OnStateExit in player");
        MAFightManager manager = GameObject.FindGameObjectWithTag("myManager").GetComponent<MAFightManager>();
        manager.DestroyAllProjectiles();
        if (animator.gameObject.tag == "myPlayer")
        {
            manager.StartCoroutine(manager.EvaluateFight(false));
            Destroy(animator.gameObject);
        }
        else
        {
            manager.SpawnChest();
            Destroy(animator.gameObject);
        }
    }
#endif
}
