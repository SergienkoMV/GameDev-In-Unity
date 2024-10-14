using System.Collections;
using UnityEngine;
using DG.Tweening;


public class PlayerMover : MonoBehaviour
{
    [SerializeField] private Transform[] _pointers;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _durationRotate = 0.5f;
    [SerializeField] private Ease _moveEasy = Ease.Linear;
    [SerializeField] Animator _animator;
    [SerializeField] private Renderer _renderer;
    [SerializeField] ParticleSystem _particleSystem;
    
    private float _currentSpeed;
    private readonly int _isWalking = Animator.StringToHash("Walk");
    private Vector3 _previousPointer;
    private float _color = 0f;
    private float _scaleSize = 1f;

    

    void Start()
    {
        _previousPointer = transform.position;
        SetCurrentSpeed(0);
        StartCoroutine(MoveToPoint());
    }

    private IEnumerator MoveToPoint()
    {
        foreach (var point in _pointers)
        {
            //���������� ����������� � ��������� ����� ��� ���������� ���� ��������
            Vector3 direction = point.position - _previousPointer;
            //��������� ���� � ����������� ��������
            float angle = Vector3.Angle(Vector3.forward, direction) * (Vector3.Dot(Vector3.up, Vector3.Cross(Vector3.forward, direction)) < 0 ? -1 : 1);
            //��������� ������� ����� �������� DOTween
            transform.DORotate(new Vector3(0, angle, 0), _durationRotate);
            //���� ���� ����������� �������� ��������
            yield return new WaitForSeconds(_durationRotate);
            //������������� �������� ��� ����������� � �������� � �������� ���� � ���������.
            SetCurrentSpeed(_speed);
            //������� �����, ����������� ��� ����������� ���� �� ��������� �����
            float time = Vector3.Distance(_previousPointer, point.position) / _speed;
            //��������� ����������� ����� �������� DOTween � � ��� ����� ��������� ������� ������, ��� �������� ����.
            //������� ������ ����� ���� ��������� � ���������� � ������� ����� ������� �����������, �� � ������� �������,
            //��� ���������� ���������� ���� ��� ����� ����� DOTween, ������� ������� �� ��������.
            transform.DOMove(point.position, time).SetEase(_moveEasy).OnStart(() => { UpdateDuringDOTween(); });

            //�� ������ ������� ���� �������� ���������� � ���������� �������� ��������� 
            if (_scaleSize == 1)
            {
                _scaleSize = 2;
            } else 
            {
                _scaleSize = 1;
            }
            //��������� ��������������� ����� �������� DOTween
            transform.DOScale(_scaleSize, time).SetEase(_moveEasy);
            //��������� ��������� ����� ����� ����� �������� DOTween
            _color += 0.2f;
            _renderer.material.DOColor(new Color(_color, _color, _color), time);
            //��������� ���������� ����� ��� ������������� � ��������� ������� ��������
            _previousPointer = point.position;
            //���� ���� ����������� �������� �����������, ��������������� � ��������� �����
            yield return new WaitForSeconds(time);
            //������������� ������� ������.
            _particleSystem.Stop();
            //�������� ��������, ��� ������������ �������� ������ �� �������� ������� �� �����
            SetCurrentSpeed(0);
        }
    }

    //������� �������� ��� ������ � �����������
    private void SetCurrentSpeed(float speed)
    {
        _currentSpeed = speed;
        _animator.SetFloat(_isWalking, _currentSpeed);
    }

    //��������� ������� ������
    private void UpdateDuringDOTween()
    {
        _particleSystem.Play();
    }
}
