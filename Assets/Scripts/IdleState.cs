using UnityEngine;

public class IdleState : AIState
{

    private float _timer;

    public AIStateID GetID()
    {
        return AIStateID.Idle;
    }

    public void Enter(AIAgent agent)
    {
        _timer = agent.Config._maxTime;
    }

    public void Update(AIAgent agent)
    {
        _timer -= Time.deltaTime;
        agent.Animator.SetFloat("Timer", _timer);

        if (_timer < 0)
        {
            agent.StateMachine.ChangeState(AIStateID.Search);
        }
    }

    public void Exit(AIAgent agent)
    {

    }
}
