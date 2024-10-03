using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _player;
    [SerializeField] private AudioSource _enemySound;
    private float _distance;
    
    void Update()
    {
        transform.position -= Vector3.forward * Time.deltaTime * _speed;
        _distance = (transform.position - _player.transform.position).magnitude;
        if (Mathf.Abs(_distance) < 50)
        {
            if (_distance == 0)
            {
                _enemySound.volume = 1;
            }
            else
            {
                 _enemySound.volume = 1 / _distance;
            }
                
        }
        else
        {
            _enemySound.volume = 0;
        }
        print(_distance);
    }
}
