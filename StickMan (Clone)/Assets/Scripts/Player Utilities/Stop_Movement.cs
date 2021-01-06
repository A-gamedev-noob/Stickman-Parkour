using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop_Movement : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) 
    {
        if(other.transform.CompareTag("Player"))
        {
            
        }
    }
}
