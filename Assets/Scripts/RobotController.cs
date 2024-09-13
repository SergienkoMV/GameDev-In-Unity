using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
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


    //iv.Двигается по комнате, избегая столкновений с препятствиями.

    // Start is called before the first frame update
    void Start()
    {
        _range += transform.localScale.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        FindGarbage();
        if (_action == 1)
        {

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
            }
            else
            {
                print("препятствие");
                if (!Physics.Raycast(rayRight, _range)) //сдеть рандомным выбор стороны rayLeft определить раньше и заменить на абстрактную переменную
                {
                    print(1);
                    _angel = transform.eulerAngles.y + _angleRotation;
                    _action = 2;
                    sideRotation = 1;                  
                }
                else//(!Physics.Raycast(rayLeft, _range))
                {
                    print(2);
                    _angel = transform.eulerAngles.y - _angleRotation;
                    _action = 2;
                    sideRotation = -1;
                }
                //if (Physics.Raycast(rayLeft, _range) && Physics.Raycast(rayRight, _range))
                //{
                //    print(3);
                //    _angel = transform.eulerAngles.y + _angleRotation * 2;
                //    sideRotation = 1;
                //    //RotateRobot(3);
                //    //Отъехать и попробовать повернуть и так много раз.
                //}
                _action = 2;
            }
        }
        else
        {
            //iii.Изменяет направление движения робота, если обнаруживает препятствие.
            RotateRobot(sideRotation);
        }
    }

    private void FindGarbage()
    {

        Collider[] collidersForCheck = Physics.OverlapSphere(transform.position +  transform.forward * _range, _radius, _targetMask);
        if (collidersForCheck.Length > 0)
        {
            print("Garbage finded");
            for (int i = 0; i < collidersForCheck.Length; i++)
            {
                collidersForCheck[i].GetComponent<Garbage>().GarbageCollect();
            }
        }
    }

    private void RotateRobot(int side)
    {
        MoveRobot(0f);
        //gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (side == 1)
        {
            
            if (transform.eulerAngles.y >= 360)
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
            if (transform.eulerAngles.y >= 270)
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

    private void MoveRobot(float speed)
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed * Time.deltaTime;
    }

    //IEnumerator RotateMove(int side)
    //{
    //    var _angel = transform.eulerAngles.y + _angleRotation * side;
        
    //    while (transform.eulerAngles.y <= _angel) 
    //    {
    //        print(transform.eulerAngles.y);
    //        transform.Rotate(Vector3.up * _rotationSpeed);
    //        //gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.up * _speed * Time.deltaTime);
    //        //int x = 1;
    //        //if (x == 2) 
    //        //{
    //        //    yield return null;
    //        //}
    //    }
    //    yield return null;
    //}

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