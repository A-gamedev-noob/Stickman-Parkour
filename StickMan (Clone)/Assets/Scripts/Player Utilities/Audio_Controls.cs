using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Controls : MonoBehaviour
{
    
    public bool _isAlive = true;
    [SerializeField] float _range = 5f, _delay = 0.2f,_factor = 5f,_constant = 0.1f;
    [SerializeField] LayerMask _layerMask;
    void Start()
    {
        _isAlive = true;
        StartCoroutine(Detect());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Detect()
    {
        while(_isAlive)
        {
            Collider2D[] col = Physics2D.OverlapCircleAll(transform.position,_range,_layerMask);
            foreach(Collider2D obj in col)
            {
                AudioSet(obj);
            }
            yield return new WaitForSeconds(_delay);
        }
    }


    void AudioSet(Collider2D obj)
    {
        float distance = Vector2.Distance(obj.transform.position,transform.position);
        float value = (_constant*_factor)/distance;
        AudioSource ASource = obj.GetComponent<AudioSource>();
        if(ASource)
        {
            ASource.spatialBlend = 0;
            ASource.volume = value;
            if (distance >= _range)
            {
                ASource.volume = 0;
            }
        }
       
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
