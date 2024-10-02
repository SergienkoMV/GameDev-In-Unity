using UnityEngine;
using UnityEngine.AI;

//Персонаж подходит к предмету и “собирает” его (можно использовать простую анимацию сбора).
public class CollectState : AIState
{
    private GameObject _item;
    NavMeshAgent agent;

    public AIStateID GetID()
    {
        return AIStateID.Collect;
    }

    public void Enter(AIAgent agent)
    {

    }

    public void Update(AIAgent agent)
    {
        if (!agent.NavMeshAgent.pathPending && agent.NavMeshAgent.remainingDistance < 0.5f)
        {
            agent.Animator.SetTrigger("ItemFound");
            agent.Item.GetComponent<Item>().ItemDestroy();
            agent.Item = null;
            agent.StateMachine.ChangeState(AIStateID.Idle);
        }
    }

    public void Exit(AIAgent agent)
    {
        agent.Animator.SetTrigger("ItemCollected");
    }
}
