using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int _score = 0;
    private int _numberTry = 1;
    private float _tryTime = 7f;
    //private int _countSkittles;
    //private int _tryThrow = 1;
    private float _mouseX;
    private float _sliderSpeed = 2.0f;
    private float _bonus;
    private bool _isRolling = false;
    private bool _forceSet = true;
    //private Vector3 _startBallPosition;
    private int _round = 0;
    private int[,] _roundsArray = new int[10, 2];


    //[SerializeField] GameObject _ballGameObject;
    //[SerializeField] Transform[] _skittles;
    [SerializeField] private GameObject _currentBall;
    [SerializeField] private BallController _ballScript;
    [SerializeField] private GameObject[] _balls;
    [SerializeField] private SkittlesController _skittlesScript;
    [SerializeField] private TextMeshProUGUI _scoreLable;
    [SerializeField] private TextMeshProUGUI _messageLable;
    [SerializeField] private Slider _throwForce;
    [SerializeField] private TextMeshProUGUI _TryLable;
    [SerializeField] private TextMeshProUGUI _roundLable;
    [SerializeField] private TMP_Dropdown _ballsChoosen;
    [SerializeField] private GameObject _menu;

    //public int Score => _score;

    //public int Turn => _numberTry;

    public bool IsRolling { get => _isRolling; set => _isRolling = value; }

    void Start()
    {
        SetNextRound(1);
        //NextRound();
        _ballScript = _currentBall.GetComponent<BallController>();
    }

    void Update()
    {
        //выбираем шар
        if (!_isRolling) 
        {
            MoveSlider();

            Vector3 _directionForRay = _ballScript.transform.forward;
            Ray ray = new Ray(_ballScript.transform.position, new Vector3 (_directionForRay.x, 0f, _directionForRay.z));
            Debug.DrawRay(ray.origin, ray.direction * 5, Color.red);

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!_menu.activeSelf)
                {
                    _menu.SetActive(true);
                    BallChoosen();
                }
                else
                {
                    _menu.SetActive(false);
                    Time.timeScale = 1f;
                }
            }

            if (Input.GetMouseButtonDown(0) && !_menu.activeSelf)
            {
                StartCoroutine(GameProcess());
            }
        } 
    }

    IEnumerator GameProcess()
    {
        _isRolling = true;
        _ballScript.ThrowBall(_throwForce.value); //добавить вращение экрана и бросать шар туда, куда смотрит экран
        yield return new WaitForSeconds(_tryTime);
        print(_skittlesScript.CountStandSkittles());
        _ballScript.BallReturn();
        CountScore();
        SetNextTry();
        _isRolling = false;
    }

    private int CountScore() //подсчет очков
    {
        if (_round > 2)
        {
            if (_roundsArray[_round - 3, 1] != 0)
            {
                _roundsArray[_round - 3, 1]--;
                _roundsArray[_round - 3, 0] += _skittlesScript.CountStandSkittles();
                _score = _score + _skittlesScript.CountStandSkittles();
            }
        }
        if (_round > 1)
        {
            if (_roundsArray[_round - 2, 1] != 0)
            {
                _roundsArray[_round - 2, 1]--;
                _roundsArray[_round - 2, 0] += _skittlesScript.CountStandSkittles();
                _score = _score + _skittlesScript.CountStandSkittles();
            }
        }
        
        _roundsArray[_round-1, 0] += _skittlesScript.CountStandSkittles();

        _score = _score + _skittlesScript.CountStandSkittles();
        print("Score: " + _score.ToString());
        if (_round > 2)
        {
            print($"Round {_round - 2} - {_roundsArray[_round - 3, 0]}");
        }
        if (_round > 1)
        {
            print($"Round {_round - 1} - {_roundsArray[_round - 2, 0]}");
        }
        print($"Round {_round} - {_roundsArray[_round-1, 0]}");

        _scoreLable.text = "Score: " + _score.ToString();
        
        return _score;
    }

    private GameObject BallChoosen() //выбор шара
    {
        Time.timeScale = 0;
        //_currentBall = _balls[2];
        _ballScript = _currentBall.GetComponent<BallController>();
        return GetComponent<GameObject>(); 
    }

    private void ShowMessage(string message)
    {
        StartCoroutine(Message(message));
    }

    IEnumerator Message(string message)
    {
        _messageLable.text = message;
        yield return new WaitForSeconds(10);
        _messageLable.text = "";
    }

    private void MoveSlider()
    {
        if (_forceSet)
        {
            _throwForce.value += _sliderSpeed * Time.deltaTime;
            if (_throwForce.value >= 1f)
            {
                _forceSet = false;
            }
        }
        else
        {
            _throwForce.value -= _sliderSpeed * Time.deltaTime;
            if (_throwForce.value <= 0f)
            {
                _forceSet = true;
            }
        }
    }

    private void SetNextTry()
    {
        if (_skittlesScript.CountStandSkittles() == 10)
        {
            if (_numberTry == 1)
            {
                ShowMessage("STRIKE");
                _roundsArray[_round - 1, 1] = 2;
                SetNextRound(1);
            }
            else
            {
                ShowMessage("SPARE");
                _roundsArray[_round - 1, 1] = 1;
                SetNextRound(1);
            }
        }
        else if (_numberTry == 1)
        {
            SetNextRound(2);
        }
        else
        {
            SetNextRound(1);
        }
    }

    private void SetNextRound(int numberTry)
    {
        _numberTry = numberTry;
        _TryLable.text = "Try: " + numberTry;
        if (numberTry == 1)
        {
            NextRound();
            _skittlesScript.SkittlesReturn();
        } 
        else
        {
            _skittlesScript.SkittlesClear();
        }
    }

    private void NextRound()
    {
        _round++;
        _roundLable.text = "Round: " + _round;
    }
}
