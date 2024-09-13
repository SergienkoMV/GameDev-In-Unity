using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushRotate : MonoBehaviour
{
    [SerializeField] private int _rotateSpeed; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
