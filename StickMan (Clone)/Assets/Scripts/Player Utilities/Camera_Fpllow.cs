using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Fpllow : MonoBehaviour
{
    
    public Transform ply;
    public Vector2 _offset;
    public float _damp = 0.125f;
    float yVelocity = 0.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        float newpos = Mathf.SmoothDamp(transform.position.y,ply.position.y + _offset.y, ref yVelocity,_damp);
        transform.position = new Vector3(transform.position.x, newpos,transform.position.z);
    }
}
