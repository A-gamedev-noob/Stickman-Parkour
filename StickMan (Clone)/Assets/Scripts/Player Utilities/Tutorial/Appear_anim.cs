using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Appear_anim : MonoBehaviour
{
    public SpriteRenderer _object;
    public SpriteRenderer _object2;
    public float _increment = 0.1f;
    public float _time = 0.05f;

    void Start()
    {
        _object.color = new Color(_object.color.r,_object.color.g,_object.color.b,0f);
    }

    IEnumerator TextUp()
    {
        while(_object.color.a<1f)
        {
            FadeIn(_object);
            yield return new WaitForSeconds(_time);
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(TextUp());
            _object2.color = new Color(_object2.color.r, _object2.color.g, _object2.color.b, 0f);
        }
    }

    void FadeIn(SpriteRenderer spriteR)
    {
        spriteR.color = new Color(spriteR.color.r, spriteR.color.g, spriteR.color.b, spriteR.color.a + _increment);
    }

}
