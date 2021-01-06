using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax_Background : MonoBehaviour
{
    public float _effect;
    public float _length = 62f;
    Vector3 _lastLoc;
    [SerializeField]float Distance;

    void Start()
    {
        _lastLoc = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(-1f, 0f, 0f) * _effect * Time.deltaTime;
        transform.position += movement;
        Distance = Vector3.Distance(_lastLoc,transform.position);
        if(Distance >= _length)
        {
            PauseEditor();
            transform.position += new Vector3(_length,0,0);
            _lastLoc = transform.position;
            PauseEditor();
        }
    }

    void PauseEditor()
    {
        Debug.Break();
    }
}
