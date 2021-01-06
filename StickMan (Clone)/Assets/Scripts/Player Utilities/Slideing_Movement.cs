using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slideing_Movement : MonoBehaviour
{
    [SerializeField] float _crouchHieght = 1.35f,_speed = 5f;
    [SerializeField] Vector2 _offset = new Vector2(1.55f,-1.5f),_posOffeset;
    Vector2 _orignalHieght,_orignalOffset;
    //[HideInInspector]
    public bool _slide;
    Player_Movement _pm;
    Rigidbody2D _rb;
    BoxCollider2D _col;
    public float _input;
    public bool _crouched;
    [SerializeField] LayerMask _obstacles;
    void Start()
    {
        _col = transform.GetComponent<BoxCollider2D>();
        _orignalHieght = _col.size;
        _orignalOffset = _col.offset;
        _rb = transform.GetComponent<Rigidbody2D>();
        _pm = transform.GetComponent<Player_Movement>();
        _crouched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_slide)
        {
            GoDown();
        }
        else if(!_slide && _col.size.y < _orignalHieght.y)
        {
            GoUp();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            _slide = true;
        }
        else if(Input.GetKeyUp(KeyCode.C))
        {
            _slide = false;
        }
        
    }

    void GoDown()
    {
        if(_pm._isgrounded)
        {
            if(_col.size.y>_crouchHieght)
            {
                _col.size = new Vector2(_orignalHieght.x,_crouchHieght);
                _pm._animator.SetTrigger("Crouch");
                _rb.position = _rb.position + _posOffeset;
                _col.offset = _offset;
                _crouched = true;
            }
             
        }
    }

    void Move()
    {
        if (_pm._moveL && !_pm._moveR) { _input = -1;}
        else if (!_pm._moveL && !_pm._moveR) { _input = 0; }
        else if (!_pm._moveL && _pm._moveR) { _input = 1; }
    }

    public void Movement()
    {
        if (_pm.KControl)
        {
            _input = Input.GetAxisRaw("Horizontal");
        }
        else
            Move();

        if (_input != 0)
        {
            _pm._animator.SetBool("Crawl", true);
            _rb.velocity = new Vector2(_input * _speed, _rb.velocity.y);
            if (_pm._facingRight && _input < 0)
            {
                transform.Rotate(0, -180f, transform.rotation.z);
                _pm._facingRight = false;
            }
            else if (!_pm._facingRight && _input > 0)
            {
                transform.Rotate(0, 180f, transform.rotation.z);
                _pm._facingRight = true;
            }
        }
        else
        {
            _pm._animator.SetBool("Crawl", false);
        }
    }

    void GoUp()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position,transform.up,1.5f,_obstacles);
        if(raycastHit.collider == null)
        {
            _col.size = _orignalHieght;
            _col.offset = _orignalOffset;
            _pm._animator.SetTrigger("Crouch");
            if (_pm._animator.GetBool("Crawl"))
                _pm._animator.SetBool("Crawl", false);
            _crouched = false;
        }
   
    }

    public void SlideBool(bool a)
    {
        _slide = a;
    }

    void Interactive()
    {

    }
}
