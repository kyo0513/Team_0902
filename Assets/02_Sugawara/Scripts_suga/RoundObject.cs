using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundObject : MonoBehaviour
{
    public float roundSpeed; 
    void Update()
    {
        transform.Rotate(0,0,this.roundSpeed);   
    } 
}
