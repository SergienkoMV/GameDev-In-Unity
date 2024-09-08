using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int _firstTryScore;
    private int _score = 0;
    private int _numberTry = 1;
    private int _round = 0;
    private int _maxRounds = 10;
    private int[,] _roundsArray = new int[10, 2];
    private float _tryTime = 7f;
    private float _mouseX;
    private float _sliderSpeed = 2.0f;
    private float _bonus;
    private bool _isRolling = false;
    private bool _forceSet = true;

    [SerializeField] private GameObject _currentBall;
    [SerializeField] private BallController _ballScript;
    [SerializeField] private GameObject[] _balls;
    [SerializeField] private SkittlesController _skittlesScript;
    [SerializeField] private TextMeshProUGUI _scoreLable;
    [SerializeField] private TextMeshProUGUI _messageLable;
    [SerializeField] private Slider _throwForce;
    [SerializeField] private TextMeshProUGUI _TryLable;
    [SerializeField] private TextMeshProUGUI _roundLable;
    [SerializeField] private TMP_Dropdown _balls—hoice;
    [SerializeField] private GameObject _menu;

    public bool IsRolling { get => _isRolling; set => _isRolling = value; }

    void Start()
    {
        SetNextRound(1);
        _ballScript = _currentBall.GetComponent<BallController>();
    }

    void Update()
    {      
        if (!_isRolling && _round <= _maxRounds) 
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
                    Time.timeScale = 0;
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

            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartScene();
            }
        } 
    }

    IEnumerator GameProcess()
    {
        _isRolling = true;
        _ballScript.ThrowBall(_throwForce.value);
        yield return new WaitForSeconds(6);
        CountScore();      
        SetNextTry();
        _isRolling = false;
    }

    private int CountScore()
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
        _scoreLable.text = "Score: " + _score.ToString();
        return _score;
    }

    public void Ball—hoice()
    {
        _currentBall.SetActive(false);
        _currentBall = _balls[_balls—hoice.value];
        _currentBall.SetActive(true);
        _ballScript = _currentBall.GetComponent<BallController>();
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
        if (_skittlesScript.CountStandSkittles() + _firstTryScore == 10)
        {
            if (_numberTry == 1)
            {
                StartCoroutine(Message("STRIKE"));
                _roundsArray[_round - 1, 1] = 2;
                SetNextRound(1);
            }
            else
            {
                print("kfjgklsdj");
                StartCoroutine(Message("SPARE"));
                _roundsArray[_round - 1, 1] = 1;
                SetNextRound(1);
                _firstTryScore = 0;
            }
        }
        else if (_numberTry == 1)
        {
            _firstTryScore = _skittlesScript.CountStandSkittles();
            SetNextRound(2);
        }
        else
        {
            _firstTryScore = 0;
            SetNextRound(1);
            }
    }

    IEnumerator Message(string message)
    {
        _messageLable.text = message;
        yield return new WaitForSeconds(3);
        _messageLable.text = "";
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
        _ballScript.BallReturn();
    }

    private void NextRound()
    {
        _round++;
        if (_round > _maxRounds)
        {
            StopAllCoroutines();
            StartCoroutine(Message("Game Over"));
            StartCoroutine(GameOver());
        }
        else
        {
            _roundLable.text = "Round: " + _round;
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(_tryTime);
        RestartScene();
    }
}
