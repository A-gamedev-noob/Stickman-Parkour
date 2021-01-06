using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Player_Movement : MonoBehaviour
{
    Rigidbody2D rb;
    float x;
    Collider2D _collider;
    [Header("Attributes")]
    public float _speed = 15,_jumpForce = 35;
    [SerializeField] float _wallSlideSpeed = 1,_wallRunDuration = 1f,_wallJumpDuration = 0.2f,
    _wallRunSpeed = 12f;
    [SerializeField] float _xWallJumpForce = 4, _yWallJumpForce = 8;
    
    [HideInInspector]public bool _isgrounded = true, _airAnimaing = false, _touchingWall = false, _canWallRun = true,_wallJumping = false,_airMovement = true,_moveL = false,_moveR = false;
    bool _wallRunning = false, _wallruncheck = true;
    public bool _jump;
    [Header("Refrences")]
    [SerializeField] Transform _groundcheck;
    [SerializeField] Transform _wallDetector;
    public GameManager _GM;
    public Level_Tracker _LT;
    public GameObject _endScreen;
    [HideInInspector] public Animator _animator;
    [SerializeField] GameObject _body;
    [SerializeField] LayerMask _ground,_climbables,_player;
    [SerializeField] GameObject[] _limbs;
    [SerializeField] GameObject _blood;
    [SerializeField] GameObject _OSC;
    [HideInInspector] public bool _star;

    [SerializeField] GameObject _cam;
    
    public bool _facingRight = true, KControl = false, _isActive = true;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _LT.gameObject.SetActive(true);
        rb.freezeRotation = true;
        _star = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isActive)
        {
            if (!KControl)
            {
                Move();
            }
            else
                x = Input.GetAxisRaw("Horizontal");
            if(!transform.GetComponent<Slideing_Movement>()._slide &&!transform.GetComponent<Slideing_Movement>()._crouched)
            {
                Movement();
                WallActions();
                Jumping();
            }
            else{
                transform.GetComponent<Slideing_Movement>().Movement();
            }    
        }
        else{
            _OSC.SetActive(false);
        }
    }

    void Movement()
    {
       
        //Running animation
        if(x != 0 && _isgrounded)
        {
            _animator.SetBool("IsRunning",true);
        }
        else
        {
            _animator.SetBool("IsRunning",false);
        }
        //Rotation Code
        if(x > 0 && !_facingRight && !_animator.GetBool("WallRun"))
        {
            transform.Translate(0.3f, 0, 0);
            transform.Rotate(0,180f,transform.rotation.z);
            _facingRight = true;
        }
        else if(x < 0 && _facingRight && !_animator.GetBool("WallRun"))
        {
            transform.Translate(-0.3f,0,0);
            transform.Rotate(0, -180f, transform.rotation.z);
            _facingRight = false;
        }
        //Movement Code
        if(_airMovement && !_animator.GetBool("WallRun"))
        {
            rb.velocity = new Vector2(x * _speed, rb.velocity.y);
        }
    }
    
    void Move()
    {
        if(_moveL && !_moveR){x = -1;}
        else if(!_moveL && !_moveR){x = 0;}
        else if(!_moveL && _moveR){x = 1;}
    }

    public void MoveBoolLeft(bool a)
    {
        _moveL = a;
    } 
    public void MoveBoolRight(bool a)
    {
        _moveR = a; 
    }


    private void Jumping()
    {
        _isgrounded = Physics2D.OverlapBox(_groundcheck.position,new Vector2(transform.lossyScale.x-0.3f, 0.1f),transform.rotation.z, _ground);
        _animator.SetBool("IsGrounded", _isgrounded);
        if (_isgrounded && (Input.GetButtonDown("Jump") || _jump) && !_touchingWall && !transform.GetComponent<Slideing_Movement>()._slide)
        {
            rb.velocity = Vector2.up * _jumpForce;
            _animator.SetTrigger("Jumping");
            _airAnimaing = true;
        }
        if(_isgrounded)
        {
            _canWallRun = true;
            _airAnimaing = false;
        }
        else if(!_isgrounded && !_airAnimaing && !_animator.GetBool("SlideingDown") && !_animator.GetBool("WallRun"))
        {
            _airAnimaing = true;
            if(!_touchingWall)
            {
                _jump = false;
            }
            _animator.SetTrigger("Jumping");

        }
    }
    void WallActions()
    {
        _touchingWall = Physics2D.OverlapBox(_wallDetector.position,new Vector2(0.1f,transform.lossyScale.y*3.6f),0f,_climbables);
        if( rb.velocity.y > -0.5f && _wallruncheck)
        {
            _canWallRun = true;
        }
        else
        {
            _canWallRun = false;
        }
       if(_touchingWall)
       {
           // Wall Running
           if(_canWallRun && x != 0)
           {
                _wallruncheck = false;
                StartCoroutine(WallRun());
                _canWallRun = false;
           }
           //Wall Slideing
           else if(!_isgrounded && !_wallJumping && rb.velocity.y < 0.8f)
           {
                rb.velocity = new Vector2(rb.velocity.x, -_wallSlideSpeed);
                _animator.SetBool("SlideingDown",true);
           }
            if((_jump || Input.GetButtonDown("Jump")) && !_isgrounded)
            {
                _wallJumping = true;
                _wallRunning = false;
                _airMovement = false;
                Invoke("AirMovementTrue",0.2f);
                Invoke("WallJumpFalse",_wallJumpDuration);
            }
       }
       else
       {
           if(!_wallruncheck)
           {
               _wallRunning = false;
               Invoke("WallRuncheck",0.1f);
           }
           _animator.SetBool("SlideingDown", false);
           _animator.SetBool("WallRun",false);
           if(!_isgrounded)
           {
               _jump = false;
           }
       }
        if (_wallJumping)
        {
            if(_facingRight && x == 0)                                                              
            {
                if(rb.velocity.y > 1f)
                {
                    rb.velocity = new Vector2(-_xWallJumpForce, _yWallJumpForce);
                }
                else{
                    rb.velocity = new Vector2(-_xWallJumpForce, 0f);
                }
            }
            else if(!_facingRight && x == 0)
            {
                if (rb.velocity.y > 1f)
                {
                    rb.velocity = new Vector2(_xWallJumpForce, _yWallJumpForce);
                }
                else
                {
                    rb.velocity = new Vector2(_xWallJumpForce, 0f);
                }
            }
            else if(_animator.GetBool("SlideingDown"))
            {  
                rb.velocity = new Vector2(_xWallJumpForce * -x, 5f);
            } 
            else if(_animator.GetBool("WallRun"))
            {
                rb.velocity = new Vector2(_xWallJumpForce * -x, _yWallJumpForce);
                if(!_facingRight)
                {
                    transform.Translate(0.3f, 0, 0);
                    transform.Rotate(0, 180f, 0f);
                    _facingRight = true;
                }
                else
                {
                    transform.Translate(-0.3f, 0, 0);
                    transform.Rotate(0, -180f, 0f);
                    _facingRight = false;
                }

            }     
        }      
    }
    IEnumerator Dieing(Vector2 contactNormal)
    {
        _cam.GetComponent<CinemachineVirtualCamera>().Follow = null;
        _LT.Timer(false);
        gameObject.GetComponent<Audio_Controls>()._isAlive = false;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        var blood = Instantiate(_blood, transform.position, Quaternion.FromToRotation(Vector2.up,contactNormal));
        rb.gravityScale = 0f;
        _isActive = false;  
        _body.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        for(int i = 0; i < _limbs.Length; i++)
        {
            GameObject limb = _limbs[i];
            limb.AddComponent<Rigidbody2D>();
            limb.GetComponent<Rigidbody2D>().gravityScale = 2;
            limb.GetComponent<Collider2D>().enabled = true;
        }
        foreach(GameObject limb in _limbs)
        {
            Rigidbody2D rgb = limb.GetComponent<Rigidbody2D>();
            if(rgb != null)
            {   
                Vector2 direction = limb.transform.position - transform.position;
                rgb.AddForce(contactNormal * 700);
            }  
        }
        _GM._gameOverUI.SetActive(true);
    }
    public void Jump()
    {
        if((_isgrounded || _touchingWall) && !_jump)
        {
            _jump = true;
        }else{
            _jump = false;
        }
    }
    IEnumerator WallRun()
    {
        _animator.SetBool("WallRun", true);
        float Elapsed = 0f;
        _wallRunning = true;
        while (Elapsed < _wallRunDuration && _wallRunning)
        {
            rb.velocity = new Vector2(rb.velocity.x, _wallRunSpeed);
            Elapsed += Time.deltaTime;

            yield return null;
        }
        _wallRunning = false;
        _animator.SetBool("WallRun",false);
        yield return new WaitForSeconds(2f);    
        _wallruncheck = true;
    }
    void WallJumpFalse()
    {
        _wallJumping = false;
        _wallRunning = false;
    }
    void AirMovementTrue()
    {
        _airMovement = true;
    }
    void WallRuncheck()
    {
        _wallruncheck = true;
    }

    void PlayerUp()
    {
        RaycastHit2D hit;
        if(!_isgrounded)
        {
            hit = Physics2D.Raycast(_groundcheck.position,Vector2.down,0.4f,_ground);
            if (hit)
            {
                _airMovement = false;
                print("hit"+ hit.collider.name);
                Vector3 Nor = new Vector3(transform.rotation.x, transform.rotation.y, hit.normal.y);
                transform.rotation = Quaternion.FromToRotation(Vector2.up, hit.normal);
                transform.rotation = Quaternion.Euler(hit.normal);
                _airMovement = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("End"))
        {
            OnEnd();
        }
        else if(other.CompareTag("Star"))
        {
            _star = true;
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("Fell"))
        {
            _LT.Timer(false);
            _isActive = false;
            _GM._gameOverUI.SetActive(true);
        }
    }

    void OnEnd()
    {
        if (_star)
            _LT.GotStar();
        _LT.Timer(false);
        rb.gravityScale = 0;
        _isActive = false;
        _OSC.SetActive(false);                                          //Dissable On-Screen controls UI
        _endScreen.SetActive(true);
        _LT.LevelComplete();
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.collider.CompareTag("Killer"))
        {
            StartCoroutine(Dieing(other.contacts[0].normal));
        }
    }
 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(_groundcheck.position, new Vector2(transform.lossyScale.x - 0.3f, 0.1f));
        Gizmos.color = Color.red;
        Gizmos.DrawCube(_wallDetector.position, new Vector2(0.1f, transform.lossyScale.y * 3.6f));
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_groundcheck.position, Vector2.down * 0.4f);
    }

}
