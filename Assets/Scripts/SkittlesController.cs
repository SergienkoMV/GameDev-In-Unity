using UnityEngine;

public class SkittlesController : MonoBehaviour
{
    [SerializeField] Rigidbody[] _skittles;
    Vector3[] _startPositions;
    private int _countSkittles;


    void Start()
    {
        _startPositions = new Vector3[10];
        for (int i = 0; i < _skittles.Length; i++)
        {
            _startPositions[i] = _skittles[i].position;
        }
    }

    
    public void SkittlesClear()
    {
        for (int i = 0; i < _skittles.Length; i++)
        {
            print(i);
            if (_skittles[i].rotation != Quaternion.Euler(0, 0, 0))
            {
                print(i);
                _skittles[i].isKinematic = true;
                _skittles[i].rotation = Quaternion.Euler(0, 0, 0);
                _skittles[i].position = _startPositions[i] + Vector3.up * 50;
            }
        }
    }
    
    public void SkittlesReturn()
    {
        for (int i = 0; i < _skittles.Length; i++)
        {
            _skittles[i].isKinematic = true;
            _skittles[i].rotation = Quaternion.Euler(0, 0, 0);
            _skittles[i].position = _startPositions[i];
            _skittles[i].isKinematic = false;
        }
    }

    public int CountStandSkittles()
    {
        _countSkittles = 0;
        for (int i = 0; i < _skittles.Length; i++)
        {
            if (_skittles[i].transform.rotation != Quaternion.Euler(0, 0, 0))
            {
                _countSkittles++;
            }
        }
        return _countSkittles;
    }
}
