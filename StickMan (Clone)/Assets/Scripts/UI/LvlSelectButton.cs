using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlSelectButton : MonoBehaviour
{
    [SerializeField] int _Lvl;
    [SerializeField] enum _state {one,two,three};
    [SerializeField] Sprite[] _presets;
    [SerializeField] Text _bestTxt;
    [SerializeField] Text _bestTimeTxt;
    public float _targetTime;

    void Start()
    {
        Check();
    }

    public void Check()
    {
        int rank = 0;
        string star = "Star"+_Lvl;                                                                                          //Star Bin
        string time = "BestTime"+_Lvl;                                                                                      //Time Bin
        string completed = "Completed"+_Lvl;                                                                                //State Bin    
        if(PlayerPrefs.GetString(completed) == "true")
        {
            rank++;
            if (PlayerPrefs.GetString(star) == "true") { rank++; }
            if (PlayerPrefs.GetFloat(time) > 2f && PlayerPrefs.GetFloat(time) < _targetTime) { rank++; }                    //if Under specified time
            TimeSpan timeSpan = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(time));
            _bestTimeTxt.text = timeSpan.ToString("mm':'ss'.'ff");
            _bestTxt.text = "Best Time: ";

        }
        else{
            _bestTimeTxt.text = "";
            _bestTxt.text = "";
        }                                                 
    
        GetComponent<Image>().sprite = _presets[rank];
    }

}
