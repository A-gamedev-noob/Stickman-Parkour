using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Menu_anim : MonoBehaviour
{
    public Animator _animator;

    public void LoadLevelSelect()
    {
        _animator.SetTrigger("Change Menu");
    }

}
