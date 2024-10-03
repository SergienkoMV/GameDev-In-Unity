using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _parrentForBullets;
    [SerializeField] private AudioSource _shotSound;

    void Update()
    {
        Shot();
    }

    private void Shot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var bullet = Instantiate(_bullet, this.transform.position, transform.rotation, _parrentForBullets);
            _shotSound.Play();
        }
    }
}


