using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public bool _paused = false;
    public GameObject _pauseMenu,_controls,_gameOverUI,_sceneFader,_ply;
    
    void Start()
    {
        
        if((_pauseMenu != null) && _controls != null)
        {
            _pauseMenu.SetActive(false);
            _gameOverUI.SetActive(false);
            _controls.SetActive(true);
        }
        _sceneFader.SetActive(true);
    }

    public void PausMenu()
    {
        if(!_paused)
        {
            _pauseMenu.SetActive(true);
            _controls.SetActive(false);
            _ply.GetComponent<Player_Movement>()._isActive = false;
            Time.timeScale = 0;
            _paused = true;
        }
        else
        {
            _pauseMenu.SetActive(false);
            _controls.SetActive(true);
            _ply.GetComponent<Player_Movement>()._isActive = true;
            Time.timeScale = 1;
            _paused = false;
        }
    }
    
    public void Restart()
    {
        if (_pauseMenu) { HideMenu(); } 
        StartCoroutine(LevelLoad(SceneManager.GetActiveScene().buildIndex));
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void NextLevel()
   {
        int i = SceneManager.GetActiveScene().buildIndex +1;
        StartCoroutine(LevelLoad(i));
   }
    public void LevelSelector(int ind)
    {
        StartCoroutine(LevelLoad(ind));
    }
    IEnumerator LevelLoad(int index)
    {
        if(_pauseMenu){ HideMenu(); }
        _sceneFader.GetComponent<Animator>().SetTrigger("Load Level");
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        if(!_ply.GetComponent<Player_Movement>()._isActive){ _gameOverUI.GetComponent<Animator>().SetTrigger("Trigger"); }
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(index);
    }

    void HideMenu()
    {
        Animator animator = _pauseMenu.GetComponent<Animator>();
        animator.SetTrigger("Out");
    }

    public void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
        GameObject[] buttons = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        for(int i=0;i<buttons.Length;i++)
        {
            LvlSelectButton LSB =  buttons[i].GetComponent<LvlSelectButton>();
            if(LSB!=null)
            {
                LSB.Check();
            }
        }
    }

}
