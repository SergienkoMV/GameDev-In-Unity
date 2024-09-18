using UnityEngine;

public class BrushRotate : MonoBehaviour
{
    [SerializeField] private int _rotateSpeed; 

    void Update()
    {
        if (_rotateSpeed >= 180)
        {
            _rotateSpeed -= 360;
        }
        _rotateSpeed += _rotateSpeed;
        var _angle = new Vector3(0, _rotateSpeed, 0);
        transform.Rotate(_angle);
    }
}
