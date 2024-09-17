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
    [SerializeField] private float _range = 1f;
    [SerializeField] private int _angleRotation = 90;
    [SerializeField] private float _rotationSpeed = 200f;
    [SerializeField] private float _radius = 1.5f;
    [SerializeField] private LayerMask _targetMask;
    private int _action = 1;
    private float _angel;
    private int sideRotation;
    int x = 2; //удалить
    private Vector3 _previousePosition;
    private float _deltaTime = 2f;
    private float _nextTime;


    //iv.Двигается по комнате, избегая столкновений с препятствиями.

    // Start is called before the first frame update
    void Start()
    {
        _range += transform.localScale.x / 2;
        _angel = transform.eulerAngles.y;
        _previousePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_action == 1)
        {
            _nextTime += Time.deltaTime;
           

            transform.Rotate(Vector3.zero);
            //ii.Использует Raycast для обнаружения препятствий впереди, слева и справа от робота.
            Ray rayForward = new Ray(transform.position, transform.forward);
            Ray rayLeft = new Ray(transform.position, -transform.right);
            Ray rayRight = new Ray(transform.position, transform.right);

            //print(transform.eulerAngles.y);
            RaycastHit hit;
            //if (Physics.Raycast(rayForward, out hit, _range))
            //{
            //    print("препятствие:" + hit.collider.name);
            //}
            
            if (!Physics.Raycast(rayForward, out hit, _range))
            {
                //i.Реализует движение робота с заданной скоростью.
                
                MoveRobot(_speed);
                if (_nextTime >= _deltaTime)
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
                        _action = Random.Range(1, 2);
                        SetupDirection(_action);

                    }
                    _nextTime = 0;
                    _previousePosition = transform.position;
                }

                //if ((transform.position.x + 0.01 <= _previousePosition.x || transform.position.x - 0.01 >= _previousePosition.x) &&
                //(Mathf.Abs(transform.position.x) + 0.01 <= _previousePosition.z || Mathf.Abs(transform.position.x) + 0.01 >= _previousePosition.z))
                //{
                    
                //}
            } 

            else
            {
                print("препятствие");
                if (!Physics.Raycast(rayRight, _range)) //сделать рандомным выбор стороны rayLeft определить раньше и заменить на абстрактную переменную
                {
                    print(1);
                    //_angel += _angleRotation;
                    //_action = 2;
                    //sideRotation = 1;
                    _action = SetupDirection(1);
                }
                else//(!Physics.Raycast(rayLeft, _range))
                {
                    print(2);
                    //_angel -= _angleRotation;
                    //_action = 2;
                    //sideRotation = -1;
                    _action = SetupDirection(2);
                }
                //if (Physics.Raycast(rayLeft, _range) && Physics.Raycast(rayRight, _range))
                //{
                //    print(3);
                //    _angel = transform.eulerAngles.y + _angleRotation * 2;
                //    sideRotation = 1;
                //    //RotateRobot(3);
                //    //Отъехать и попробовать повернуть и так много раз.
                //}
                //_action = 2;
            }
            
        }
        else
        {
            //iii.Изменяет направление движения робота, если обнаруживает препятствие.
            RotateRobot(sideRotation);
        }
        FindGarbage();
    }


    private void MoveRobot(float speed)
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed * Time.deltaTime;
    }

    private int SetupDirection(int _action)
    {
        if (_action == 1)
        {
            _angel += _angleRotation;
            sideRotation = 1;
        }
        else
        {
            _angel -= _angleRotation;
            sideRotation = -1;
        }
        return _action = 2;
    }

    private void RotateRobot(int side)
    {
        MoveRobot(0f);
        //gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (side == 1)
        {
            
            if (_angel >= 360 && transform.eulerAngles.y <= 90)
            {
                _angel -= 360;
            }
            if (transform.eulerAngles.y <= _angel)
            {
                print(_angel + " + " + transform.eulerAngles.y);
                transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime * side);
                //gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.up * _speed * Time.deltaTime);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, _angel, 0);
                print(_angel + " установили " + transform.eulerAngles.y);
                _action = 1;
            }
            //if (transform.eulerAngles.y >= 270) 
            //{

            //}
            //else
            //{
            //    if (transform.eulerAngles.y <= _angel)
            //    {
            //        print(_angel + " + " + transform.eulerAngles.y);
            //        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime * side);
            //        //gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.up * _speed * Time.deltaTime);
            //    }
            //    else
            //    {
            //        transform.eulerAngles = new Vector3(0, _angel, 0);
            //        print(_angel + " установили " + transform.eulerAngles.y);
            //        _action = 1;
            //    }
            //}
        } else
        {
            if (_angel <= 0 && transform.eulerAngles.y >= 270)
            {
                _angel += 360;
            }
            if (transform.eulerAngles.y >= _angel)
            {
                print(_angel + " + " + transform.eulerAngles.y);
                transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime * side);
                //gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.up * _speed * Time.deltaTime);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, _angel, 0);
                print(_angel + " установили " + transform.eulerAngles.y);
                _action = 1;
            }
        }
    }

    private void FindGarbage()
    {

        //Garbage[] _garbege = FindObjectsOfType<Garbage>();
        //ComponentSearcher<Garbege>.InRadius(currentPosition, searchRadius, out componentsInRange); ;

        Collider[] collidersForCheck = Physics.OverlapSphere(transform.position + transform.forward * _range, _radius, _targetMask);
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
            Destroy(other.gameObject);
            //other.GetComponent<Garbage>().GarbageCollect(transform.position);
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