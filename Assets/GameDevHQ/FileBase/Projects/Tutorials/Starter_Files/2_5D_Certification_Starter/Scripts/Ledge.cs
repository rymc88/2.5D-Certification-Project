using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField] private GameObject _handTarget;
    [SerializeField] private GameObject _standTarget;
    private Vector3 _handPos;
    private Vector3 _standPos;

    private void Start()
    {
        if(_handTarget != null)
        {
            _handPos = _handTarget.transform.position;
        }

        if(_standTarget != null)
        {
            _standPos = _standTarget.transform.position;
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LedgeGrabChecker"))
        {
            var player = other.transform.parent.GetComponent<Player>();
           

            if(player != null)
            {
                player.GrabLedge(_handPos, this);
            }
        }
    }

    public Vector3 GetStandPos()
    {
        return _standPos;
    }
}
