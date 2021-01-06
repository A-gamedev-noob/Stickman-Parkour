using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Tracker : MonoBehaviour
{
     
    public Text _currentTimeTxt;
    public Text _bestTimeTxt;
    string time;
    public string _lvl;
    public float _targetTime;
    TimeSpan _timeplaying;
    [HideInInspector]public bool _timerGoing;
    [HideInInspector]public float _elapsedTime;

    private void Start() 
    {
        _currentTimeTxt.text ="00:00.00";
        time = "BestTime" + _lvl;
        _timeplaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(time));
        _bestTimeTxt.text = "Best time : " + _timeplaying.ToString("mm':'ss'.'ff");
        _elapsedTime = 0f;
        Timer(true);
    }

    public void Timer(bool timer)
    {
        _timerGoing = timer;
        if(timer)
            StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        string timePlaying;
        while(_timerGoing)
        {
            _elapsedTime += Time.deltaTime;
            _timeplaying = TimeSpan.FromSeconds(_elapsedTime);
            timePlaying = _timeplaying.ToString("mm':'ss'.'ff");
            _currentTimeTxt.text = timePlaying;
            yield return null;
        }
    }

    public void GotStar()
    {
        PlayerPrefs.SetString("Star"+_lvl,"true");
        print("Star collect "+PlayerPrefs.GetInt("Star"+_lvl));
    }

    public void LevelComplete()
    {
        PlayerPrefs.SetString("Completed"+_lvl,"true");
        if (_elapsedTime < PlayerPrefs.GetFloat(time))
        {
            PlayerPrefs.SetFloat(time, _elapsedTime);
        }
        else if (PlayerPrefs.GetFloat(time) < 2f)
        {
            PlayerPrefs.SetFloat(time, _elapsedTime);
        }
    }

}
