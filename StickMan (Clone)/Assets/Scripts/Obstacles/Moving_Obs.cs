using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Obs : MonoBehaviour
{
   
    [SerializeField] Transform _destination1, _destination2;
    Transform _nextPos;
    [SerializeField] float _speed = 5f;
    [SerializeField] bool _canParent = true;

   
    void Start()
    {
        transform.position = _destination1.position;
        _nextPos = _destination2;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if(transform.position != _nextPos.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _nextPos.position, _speed*Time.deltaTime);
        }
        else{
            if(transform.position == _destination1.position)
            {
                _nextPos = _destination2;
            }
            else if(transform.position == _destination2.position)
            {
                _nextPos = _destination1;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(_canParent)
        {
            other.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D other) 
    {
        if(_canParent)
        {
            other.transform.transform.parent = null;
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_destination1.position, 0.5f);
        Gizmos.DrawWireSphere(_destination2.position, 0.5f);
    }

}
