using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField] private float _speed = 10f;
    private Rigidbody _ball;
    private bool _active = false;

    void Start()
    {
        _ball = GetComponent<Rigidbody>();
        
    }

    void Update()
    {
        if(!_active && Input.GetMouseButton(0))
        {
            ThrowBall();
            _active = true; 
        }
    }


    private void ThrowBall()
    {
        _ball.AddForce(0, 0, _speed, ForceMode.VelocityChange);
        _speed += _speed;
    }
}
