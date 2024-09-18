using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

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
    int x = 2; //удалить
    private Vector3 _previousePosition;
    private float _downtime = 1f;
    private float _delayTime;
    


    //iv.Двигается по комнате, избегая столкновений с препятствиями.

    // Start is called before the first frame update
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
            //ii.Использует Raycast для обнаружения препятствий впереди, слева и справа от робота.
            Ray rayForward = new Ray(transform.position, transform.forward);
            Debug.DrawRay(transform.position, transform.forward * 0.6f);
            Ray rayLeft = new Ray(transform.position, -transform.right);
            Debug.DrawRay(transform.position, -transform.right * 0.7f);
            Ray rayRight = new Ray(transform.position, transform.right);
            Debug.DrawRay(transform.position, transform.right * 0.7f);

            RaycastHit hit;
          
            if (!Physics.Raycast(rayForward, out hit, _hitRrange))
            {
                //i.Реализует движение робота с заданной скоростью.
                MoveRobot(_speed);
                if (_delayTime >= _downtime)
                {
                    if(Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(_previousePosition.x)) <= 0.01 && Mathf.Abs((Mathf.Abs(transform.position.z) - Mathf.Abs(_previousePosition.z))) <= 0.01)
                    {
                        float xCurrent = Mathf.Abs(transform.position.x);
                        float xCPreviouse =  Mathf.Abs(_previousePosition.x);
                        print(xCurrent + " - " + xCPreviouse);
                        float xDelta = Mathf.Abs(transform.position.x) - Mathf.Abs(_previousePosition.x);
                        float zCurrent = Mathf.Abs(transform.position.z);
                        float zCPreviouse = Mathf.Abs(_previousePosition.z);
                        print(zCurrent + " - " + zCPreviouse);
                        float zDelta = Mathf.Abs(transform.position.z) - Mathf.Abs(_previousePosition.z);
                        print(xDelta + " " + zDelta);
                        print("Somthing in front");
                        int _side = Random.Range(0, 3);
                        print(_side);
                        _action = SetupDirection(_side);
                    }
                    _delayTime = 0;
                    _previousePosition = transform.position;
                }
            } 

            else
            {
                print("препятствие");
                if (Physics.Raycast(rayRight, _hitRrange + 0.1f))
                {
                    print(1);
                    _action = SetupDirection(2);
                }
                else if(Physics.Raycast(rayLeft, _hitRrange + 0.1f))
                {
                    print(2);
                    _action = SetupDirection(1);
                }
                else
                {
                    print(3);
                    _action = SetupDirection(Random.Range(1, 3));
                }
            }  
        }
        else
        {
            //iii.Изменяет направление движения робота, если обнаруживает препятствие.
            RotateRobot(sideRotation);
        }
        //FindGarbage();
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

    private void FindGarbage()
    {

        //Garbage[] _garbege = FindObjectsOfType<Garbage>();
        //ComponentSearcher<Garbege>.InRadius(currentPosition, searchRadius, out componentsInRange); ;

        Collider[] collidersForCheck = Physics.OverlapSphere(transform.position + transform.forward * _hitRrange, _radius, _targetMask);
        if (collidersForCheck.Length > 0)
        {
            //print("Garbage finded");
            for (int i = 0; i < collidersForCheck.Length; i++)
            {
                //collidersForCheck[i].GetComponent<Garbage>().GarbageCollect();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Garbage>())
        {
            //Destroy(other.gameObject);
            other.GetComponent<Garbage>().GarbageCollect(transform.position);
        }
    }
}

//var _angel = transform.eulerAngles.y + _angleRotation;
//if (transform.eulerAngles.y <= _angel)
//{
//    print(transform.eulerAngles.y);
//    transform.Rotate(Vector3.up * _rotationSpeed);
//    //gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.up * _speed * Time.deltaTime);
//    //int x = 1;
//    //if (x == 2) 
//    //{
//    //    yield return null;
//    //}
//}