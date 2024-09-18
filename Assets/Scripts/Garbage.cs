using UnityEngine;

public class Garbage : MonoBehaviour
{
    private Vector3 _sideMove;
    private float _moveSpeed;
    private float _lifeTime;
    private float _timeForDestroy = 1;
    private bool _forDestroy = false;
    [SerializeField] private AudioSource _AudioSource;

    private void Update()
    {
        if (_forDestroy)
        {
            _AudioSource.Play();
            this.transform.Translate(_sideMove * Time.deltaTime);
            _lifeTime += Time.deltaTime;

            if (_lifeTime >= _timeForDestroy)
            {
                Destroy(gameObject);
            }
        }
    }

    public void GarbageCollect(Vector3 position)
    {
        _sideMove = (position - transform.position);
        _sideMove = new Vector3(_sideMove.x, 0, _sideMove.z);
        _forDestroy = true;
    }
}
