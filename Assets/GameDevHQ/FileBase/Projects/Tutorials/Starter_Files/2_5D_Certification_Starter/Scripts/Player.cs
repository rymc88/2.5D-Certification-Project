using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private float _speed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpHeight;
    private Vector3 _direction;
    private Animator _anim;
    private bool _jumping = false;
    private bool _rolling = false;
    private bool _onLedge = false;
    private Ledge _currentLedge;
    


    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
    }

    
    void Update()
    {
        if (_onLedge == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _anim.SetTrigger("ClimbUp");
            }
        }

        CalculateMovement();
    }

    void CalculateMovement()
    {
        if (_controller.isGrounded == true)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            _direction = new Vector3(horizontalInput, 0, 0) * _speed;
            _anim.SetFloat("Speed", Mathf.Abs(horizontalInput));

            Vector3 facing = transform.localEulerAngles;

            if (_direction.x > 0)
            {
                facing.y = 90;
            }
            else if (_direction.x < 0)
            {
                facing.y = -90;
            }

            transform.localEulerAngles = facing;

            if(horizontalInput != 0 && Input.GetKeyDown(KeyCode.LeftShift))
            {
                _rolling = true;
                _anim.SetTrigger("RollForward");
           
            }

            if (_jumping == true) //just landed
            {
                _jumping = false;
                _anim.SetBool("Jumping", _jumping);
            }

            if (Input.GetKeyDown(KeyCode.Space) && _rolling == false)
            {
                _direction.y += _jumpHeight;

                if(horizontalInput == 0)
                {
                    _anim.SetTrigger("IdleJump");
                }
                else
                {
                    _jumping = true;
                    _anim.SetBool("Jumping", _jumping);
                }
                
            }

        }
        _direction.y -= _gravity * Time.deltaTime;

        _controller.Move(_direction * Time.deltaTime);
        
    }

    public void GrabLedge(Vector3 handPos, Ledge currentLedge)
    {
        _controller.enabled = false;
        _anim.SetBool("GrabLedge", true);
        _anim.SetFloat("Speed", 0f);
        _anim.SetBool("Jumping", false);

        _onLedge = true;

        transform.position = handPos;
        _currentLedge = currentLedge;
    }

    public void ClimbUpComplete()
    {
        transform.position = _currentLedge.GetStandPos();

        _anim.SetBool("GrabLedge", false);
        _controller.enabled = true;
    }

    public void RollComplete()
    {
        _rolling = false;
    }
}



