using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    private Vector3 _cleanerPosition;
    private Vector3 _sideMove;
    private float _moveSpeed;
    private float _lifeTime;
    private float _timeForDestroy = 2;
    private GameObject _gameObject;

    //private void Start()
    //{
    //    _gameObject = GetComponent<gameObject>();
    //}

    private void Update()
    {
        this.transform.position += _sideMove / 100;
        _lifeTime += Time.deltaTime;
        if(_lifeTime >= _timeForDestroy)
        {
            print("”ничтожить");
            Destroy(gameObject);
        }
    }

    public void GarbageCollect(Vector3 position)
    {
        print("collect");
        _cleanerPosition = position;
        _sideMove = (position - transform.position);
        _sideMove = new Vector3(_sideMove.x, 0, _sideMove.z);
        print(_sideMove);
    }
}
