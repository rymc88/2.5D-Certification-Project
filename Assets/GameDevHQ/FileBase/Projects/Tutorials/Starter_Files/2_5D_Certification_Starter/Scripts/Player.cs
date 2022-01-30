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
    [SerializeField] private ToxicityBar _toxicityBar;
    [SerializeField] private int _medicineAmount;
    private bool _climbingLadder = false;
    [SerializeField] private float _climbSpeed;
    private GameObject _ledgeGrabChecker;
    private GameObject _ladderEnterChecker;
    private bool _movingLeft = false;
    private bool _jumpingOver = false;
    private float _lastY;
    [SerializeField] private float _fallingThreshold;
    private bool _falling = false;
    [SerializeField] private float _pushPower;
    [SerializeField] private float _horizontalInput;

    
    
    
    
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
        _ledgeGrabChecker = GameObject.Find("Ledge_Grab_Checker");
        _ladderEnterChecker = GameObject.Find("Ladder_Enter_Checker");

        _lastY = transform.position.y;

        if(_ladderEnterChecker == null)
        {
            Debug.Log("Ladder Enter is Null");
        }

        if(_ledgeGrabChecker == null)
        {
            Debug.Log("Ledge Grab Checker is Null");
        }

        if(_toxicityBar == null)
        {
            Debug.Log("Toxicity Bar is Null");
        }
    }

    
    void Update()
    {
        IsFalling();

        if(_falling == true)
        {
            Falling();
       
        }
        else
        {
            Landed();
        }

        if (_onLedge == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _anim.SetTrigger("ClimbUp");
            }
        }
        else
        {

        }

        if (_climbingLadder == true)
        {
            LadderMovement();
        }

        if (Input.GetKey(KeyCode.RightShift))
        {
            _toxicityBar.ToxicExposure(1);
        }

        if (!_jumpingOver)
        {
            CalculateMovement();
        }
        
    }

    void CalculateMovement()
    {
        if (_controller.isGrounded == true)
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _direction = new Vector3(_horizontalInput, 0, 0) * _speed;
            _anim.SetFloat("Speed", Mathf.Abs(_horizontalInput));
            _anim.SetBool("Jumping", false);

            Vector3 facing = transform.localEulerAngles;

            if (_direction.x > 0)
            {
                facing.y = 90;
                _movingLeft = false;
            }
            else if (_direction.x < 0)
            {
                facing.y = -90;
                _movingLeft = true;
            }

            transform.localEulerAngles = facing;

            if(_horizontalInput != 0 && Input.GetKeyDown(KeyCode.LeftShift))
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
                

                if(_horizontalInput == 0)
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

        if(_climbingLadder == false)
        {
            _direction.y -= _gravity * Time.deltaTime;
        }

        
        _controller.Move(_direction * Time.deltaTime);
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (_horizontalInput != 0 && Input.GetKey(KeyCode.Z))
        {
            if (hit.transform.tag == "MovingBox")
            {
                Rigidbody rigidbody = hit.collider.GetComponent<Rigidbody>();

                if (rigidbody == null)
                {
                    Debug.Log("No Rigidbody on Moving Box");
                    return;
                }

                Vector3 pushDir = new Vector3(_horizontalInput, 0, 0);
                rigidbody.velocity = pushDir * _pushPower;
                _anim.SetBool("Pushing", true);

            }
        }
        else
        {
            _anim.SetBool("Pushing", false);
        }

        
    }

    private void LadderMovement()
    {
        var verticalInput = Input.GetAxisRaw("Vertical");
        _direction = new Vector3(0, verticalInput, 0) * _climbSpeed;

        if (verticalInput != 0)
        {
            _anim.speed = 1;
        }
        else 
        {
            _anim.speed = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _anim.speed = 1.0f;
            _anim.SetBool("ClimbLadder", false);
            _anim.SetBool("Jumping", true);
            _anim.SetTrigger("JumpOffLadder");
            _climbingLadder = false;
            _direction.y += _jumpHeight;
            StartCoroutine(JumpOffLadder());
           
        }

        _controller.Move(_direction * Time.deltaTime);
    }

    public void GrabLedge(Vector3 handPos, Ledge currentLedge)
    {
        _controller.enabled = false;
        _anim.SetBool("GrabLedge", true);
        _anim.SetFloat("Speed", 0f);
        _anim.SetBool("Jumping", false);
        _anim.SetBool("Falling", false);

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Medicine")
        {
            _toxicityBar.ToxicCure(_medicineAmount);
            Destroy(other.gameObject);
        }
    }

    public void PlayerDeath()
    {
        _anim.SetTrigger("Dead");
    }

    public void ClimbLadder()
    {
        _anim.SetBool("ClimbLadder", true);
        _climbingLadder = true;
        _anim.speed = 0;
    }


    public void LadderExit()
    {
        _climbingLadder = false;
        _anim.SetBool("ClimbLadder", false);
        _anim.SetTrigger("ClimbLadderToTop");
        _controller.enabled = false;
    }

    public void FinishedLadderClimb()
    {
        _controller.enabled = true;
        transform.position += new Vector3(4.5f, 5.5f, 0f);
    }

    private IEnumerator JumpOffLadder()
    {
        _ledgeGrabChecker.SetActive(false);
        _ladderEnterChecker.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        _ledgeGrabChecker.SetActive(true);
        _ladderEnterChecker.SetActive(true);
    }

    public void LadderBottomExit()
    {
        _climbingLadder = false;
        _anim.SetBool("ClimbLadder", false);
    }

    public void JumpOver()
    {
        _jumpingOver = true;
        _controller.enabled = false;
        _anim.SetBool("JumpOver",true);
        
    }

    public void JumpOverFinished()
    {
        _anim.SetBool("JumpOver", false);
        _jumpingOver = false;
        _controller.enabled = true;

        if(_movingLeft == true)
        {
            transform.position -= new Vector3(15.0f, 0f, 0f);
        }
        else
        {
            transform.position += new Vector3(15.0f, 0f, 0f);
        }
        
    }

    public void IsFalling()
    {
        float distanceSinceLastFrame = (transform.position.y - _lastY) * Time.deltaTime;
        _lastY = transform.position.y;
       

        if (distanceSinceLastFrame < _fallingThreshold)
        {
            _falling = true;
        }
        else
        {
            _falling = false;
        }
    }

    public void Falling()
    {
        _ledgeGrabChecker.SetActive(false);
        _anim.SetBool("Falling",true);
        
    }

    public void Landed()
    {
        _ledgeGrabChecker.SetActive(true);
        _anim.SetBool("Falling", false);
        _anim.SetBool("Landed",true);
    }

    public void Pushing()
    {
        _anim.SetBool("Pushing", true);
    }


}



