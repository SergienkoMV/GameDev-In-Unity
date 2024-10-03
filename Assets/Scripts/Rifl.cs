using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifl : MonoBehaviour
{
    private float xMove;
    private float yMove;
    [SerializeField] private AudioSource _reload;

    void Update()
    {
        xMove = Input.GetAxis("Mouse X");
        yMove = Input.GetAxis("Mouse Y") * -1;
        transform.Rotate(yMove, 0, xMove);

        Reload();
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _reload.Play();
        }
    }
}
