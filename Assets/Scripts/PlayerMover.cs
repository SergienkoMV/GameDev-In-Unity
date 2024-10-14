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
            //Определяем направление к следующей точке для вычисления угла поворота
            Vector3 direction = point.position - _previousPointer;
            //вычисляем угол и направление поворота
            float angle = Vector3.Angle(Vector3.forward, direction) * (Vector3.Dot(Vector3.up, Vector3.Cross(Vector3.forward, direction)) < 0 ? -1 : 1);
            //выполняем поворот через анимацию DOTween
            transform.DORotate(new Vector3(0, angle, 0), _durationRotate);
            //ждем пока выполниться анимация поворота
            yield return new WaitForSeconds(_durationRotate);
            //устанавливаем скорость для перемещения и перехода к анимации шага в аниматоре.
            SetCurrentSpeed(_speed);
            //считаем время, необходимое для перодоления пути до следующей точки
            float time = Vector3.Distance(_previousPointer, point.position) / _speed;
            //выполняем перемещение через анимацию DOTween и в это время запускаем систему частиц, для имитации пыли.
            //систему частиц можно было зациклить в инспекторе и вызвать перед началом перемещения, но в задании сказано,
            //что необходимо реализвать пыль или следы через DOTween, другого способа не придумал.
            transform.DOMove(point.position, time).SetEase(_moveEasy).OnStart(() => { UpdateDuringDOTween(); });

            //На каждом участке пути чередуем увеличение и уменьшение размеров персонажа 
            if (_scaleSize == 1)
            {
                _scaleSize = 2;
            } else 
            {
                _scaleSize = 1;
            }
            //выполняем масштабирование через анимацию DOTween
            transform.DOScale(_scaleSize, time).SetEase(_moveEasy);
            //выполняем изменение цвета волос через анимацию DOTween
            _color += 0.2f;
            _renderer.material.DOColor(new Color(_color, _color, _color), time);
            //сохраняем предыдущую точку для использования в следующем отрезке маршрута
            _previousPointer = point.position;
            //ждем пока выполниться анимация перемещения, масштабирования и изменения цвета
            yield return new WaitForSeconds(time);
            //Останавливаем систему частиц.
            _particleSystem.Stop();
            //изменяем скорость, для переключения анимации хотьбы на анимацию стояния на месте
            SetCurrentSpeed(0);
        }
    }

    //Изменям скорость для работы с аниаматором
    private void SetCurrentSpeed(float speed)
    {
        _currentSpeed = speed;
        _animator.SetFloat(_isWalking, _currentSpeed);
    }

    //запускаем систему частиц
    private void UpdateDuringDOTween()
    {
        _particleSystem.Play();
    }
}
