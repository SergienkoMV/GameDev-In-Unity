using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField] private float _speed = 5f;
    //[SerializeField] private float _gameTime = 5f;
    private Rigidbody _ball;
    private bool _active = false;
    private GameController gameContrioller;
    private float _velocityBall;
    private Vector3 _startPosition;
    

    public float VelocityBall => _velocityBall;
    public bool Active => _active;

    void Start()
    {
        _ball = GetComponent<Rigidbody>();
        gameContrioller = GetComponent<GameController>();
        _startPosition = _ball.position;
    }

    public void ThrowBall(float _sliderForce)
    {
        _active = true;
        _ball.AddRelativeForce(0, 0, _speed * _sliderForce, ForceMode.Impulse);
    }

    public void BallReturn()
    {
        _ball.position = _startPosition;
    }

}
