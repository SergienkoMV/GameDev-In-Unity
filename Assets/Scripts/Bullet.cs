using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    private AudioSource _impactSound;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _impactSound = GameObject.Find("HitSound").GetComponent<AudioSource>();
    }

    void Update()
    {
        rb.AddForce(transform.forward, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _impactSound.Play();
        print(collision.collider.name);
    }
}
