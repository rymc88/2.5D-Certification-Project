using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private int _targetID;
    [SerializeField] private float _speed;
    [SerializeField] private float _elevatorDelay;
    private bool _moving = true;
    
    void Start()
    {
        _targetID = (_targetID + 1) % _wayPoints.Length;
    }

    private void FixedUpdate()
    {
        if(_moving == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _wayPoints[_targetID].transform.position, _speed * Time.deltaTime);
        }
        
        if(transform.position == _wayPoints[_targetID].transform.position)
        {
            _targetID = (_targetID + 1) % _wayPoints.Length;
            StartCoroutine(ElevatorDelay());      
        }
        
    }

    IEnumerator ElevatorDelay()
    {
        _moving = false;
        yield return new WaitForSeconds(_elevatorDelay);
        _moving = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
