using UnityEngine;

public class RobotController : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _hitRrange = 1f;
    [SerializeField] private int _angleRotation = 90;
    [SerializeField] private float _rotationSpeed = 200f;
    [SerializeField] private float _radius = 1.5f;
    [SerializeField] private LayerMask _targetMask;
    private int _action = 1;
    private float _objectiveAngel;
    private int sideRotation;
    private Vector3 _previousePosition;
    private float _downtime = 1f;
    private float _delayTime;
    


    //iv.ƒвигаетс€ по комнате, избега€ столкновений с преп€тстви€ми.

    void Start()
    {
        _hitRrange = transform.localScale.x * 0.6f;
        _objectiveAngel = transform.eulerAngles.y;
        _previousePosition = transform.position;
    }

    void Update()
    {
        if (_action == 1)
        {
            _delayTime += Time.deltaTime;
            //ii.»спользует Raycast дл€ обнаружени€ преп€тствий впереди, слева и справа от робота.
            Ray rayForward = new Ray(transform.position, transform.forward);
            Debug.DrawRay(transform.position, transform.forward * 0.6f);
            Ray rayLeft = new Ray(transform.position, -transform.right);
            Debug.DrawRay(transform.position, -transform.right * 0.7f);
            Ray rayRight = new Ray(transform.position, transform.right);
            Debug.DrawRay(transform.position, transform.right * 0.7f);

            RaycastHit hit;
          
            if (!Physics.Raycast(rayForward, out hit, _hitRrange))
            {
                //i.–еализует движение робота с заданной скоростью.
                MoveRobot(_speed);
                if (_delayTime >= _downtime)
                {
                    if(Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(_previousePosition.x)) <= 0.01 && Mathf.Abs((Mathf.Abs(transform.position.z) - Mathf.Abs(_previousePosition.z))) <= 0.01)
                    {
                        _action = SetupDirection(Random.Range(1, 3));
                    }
                    _delayTime = 0;
                    _previousePosition = transform.position;
                }
            } 

            else
            {
                if (Physics.Raycast(rayRight, _hitRrange + 0.1f))
                {
                    _action = SetupDirection(2);
                }
                else if(Physics.Raycast(rayLeft, _hitRrange + 0.1f))
                {
                    _action = SetupDirection(1);
                }
                else
                {
                    _action = SetupDirection(Random.Range(1, 3));
                }
            }  
        }
        else
        {
            //iii.»змен€ет направление движени€ робота, если обнаруживает преп€тствие.
            RotateRobot(sideRotation);
        }
    }


    private void MoveRobot(float speed)
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed * Time.deltaTime;
    }

    private int SetupDirection(int _action)
    {
        if (_action == 1)
        {
            _objectiveAngel += _angleRotation;
            sideRotation = 1;
        }
        else
        {
            _objectiveAngel -= _angleRotation;
            sideRotation = -1;
        }
        return _action = 2;
    }

    private void RotateRobot(int side)
    {
        MoveRobot(0f);
        if (side == 1)
        {
            
            if (_objectiveAngel >= 360 && transform.eulerAngles.y <= 90)
            {
                _objectiveAngel -= 360;
            }
            if (transform.eulerAngles.y <= _objectiveAngel)
            {
                transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime * side);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, _objectiveAngel, 0);
                _action = 1;
            }
        } else
        {
            if (_objectiveAngel <= 0 && transform.eulerAngles.y >= 270)
            {
                _objectiveAngel += 360;
            }
            if (transform.eulerAngles.y >= _objectiveAngel)
            {
                transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime * side);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, _objectiveAngel, 0);
                _action = 1;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Garbage>())
        {
            other.GetComponent<Garbage>().GarbageCollect(transform.position);
        }
    }
}