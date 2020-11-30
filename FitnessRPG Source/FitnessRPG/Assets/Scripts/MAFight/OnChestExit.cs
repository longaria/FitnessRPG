using UnityEngine;

public class OnChestExit : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Open", false);
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Item Icons/TreasureChest/tutorial3");
        MAFightManager manager = GameObject.FindGameObjectWithTag("myManager").GetComponent<MAFightManager>();
        manager.SpawnParticle(new Vector3(animator.transform.position.x, animator.transform.position.y + 2, animator.transform.position.z), "MagicalSource");
        manager.StartCoroutine(manager.EvaluateFight(true));
    }
}
