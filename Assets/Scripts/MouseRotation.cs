using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    private float _rotationLimit = 15;
    private float _rotationX = 0;
    private float _rotationSpeed = 100;
    private GameController _gameController;
    
    
    void Start()
    {
        _gameController = FindFirstObjectByType<GameController>();
    }

    void Update()
    {
        
        if (!_gameController.IsRolling)
        {
            _rotationX += Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime;
            //Через Lerp не получилось, так как работает только со значениями от 0 до 1. Есть ли еще какая-то функция, чтобы ограничить диапазон значений?
            if (_rotationX >= _rotationLimit)
            {
                _rotationX = _rotationLimit;
            } 
            else if (_rotationX <= -_rotationLimit)
            {
                _rotationX = -_rotationLimit;
            }
            transform.rotation = Quaternion.Euler(50, _rotationX, 0);
        }
    }
}
