using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Персонаж перемещается по случайному маршруту с помощью NavMeshAgent, ища предметы.
public class SearchState : AIState
{
    private Collider[] _hitCollider;

    public AIStateID GetID()
    {
        return AIStateID.Search;
    }

    public void Enter(AIAgent agent)
    {

    }

    public void Update(AIAgent agent)
    {
        if (!agent.NavMeshAgent.pathPending && agent.NavMeshAgent.remainingDistance < 0.5f)
        {
            if (agent.Pointers.Length != 0)
            {
                int _index = Random.Range(0, agent.Pointers.Length);
                agent.NavMeshAgent.destination = agent.Pointers[_index].position;
            }
        }

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                agent.NavMeshAgent.destination = hit.point;
            }
        }

        _hitCollider = Physics.OverlapSphere(agent.transform.position, agent.Config._searchDistance, agent.LayerMask);
        if(_hitCollider.Length > 0)
        {
            agent.NavMeshAgent.destination = _hitCollider[0].transform.position;
            agent.Item = _hitCollider[0].gameObject;
            agent.StateMachine.ChangeState(AIStateID.Collect);
        }
    }

    public void Exit(AIAgent agent)
    {

    }
}
