using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour
{
    
    Animator _animator;
    [SerializeField] GameObject _fader;
    [SerializeField] Level_Tracker _lt;
    [SerializeField] Player_Movement _ply;
    [SerializeField] GameObject[] _Starcollected;
    [SerializeField] GameObject[] _UnderTime;
    [SerializeField] Text _targetTimeText;
    TimeSpan _timspan;

    void Start()
    {
        _animator = GetComponent<Animator>();
        for (int i = 0; i < 2; i++)
        {
            _Starcollected[i].SetActive(false);
            _UnderTime[i].SetActive(false);
        }

        StartCoroutine(CheckNu());
    }

    IEnumerator CheckNu()
    {
        _timspan = TimeSpan.FromSeconds(_lt._targetTime);
        string time = _timspan.ToString("mm':'ss'.'ff");
        _targetTimeText.text += " " + time;
        
        float delay = 2f;
        int startick = 1, timetick = 1, rank = 1; 
        _fader.SetActive(true);
        yield return new WaitForSeconds(delay);
        Ease ease = Ease.InOutExpo;
        if (_ply._star)
        {
            startick = 0;
            rank++;
        }
        _Starcollected[startick].SetActive(true);
        _Starcollected[startick].transform.DOScale(1, 0.3f).SetEase(ease);
        yield return new WaitForSeconds(0.4f);
        if (_lt._elapsedTime < _lt._targetTime)
        {
            timetick = 0;
            rank++;
        }
        _UnderTime[timetick].SetActive(true);
        _UnderTime[timetick].transform.DOScale(1, 0.3f).SetEase(ease);
        yield return new WaitForSeconds(0.4f);
        _animator.SetInteger("Rank",rank);
        
    }

}
