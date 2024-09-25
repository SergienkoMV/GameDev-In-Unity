using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : MonoBehaviour
{
    //Персонаж остается на месте.
    [SerializeField] private float _maxTime = 6f;
    private Animator _animator;
    private NavMeshAgent _agent;
    [SerializeField] private float _timer;
   

    void Start()
    {
        _animator = GetComponent<Animator>();
        _timer = _maxTime;


    }

    void Update()
    {
        _timer += Time.deltaTime;
        _animator.SetFloat("Timer", _timer);

        if ( _timer > _maxTime)
        {
            _timer = 0;
            //_agent.destination = transform.position;
        }
    }
}
