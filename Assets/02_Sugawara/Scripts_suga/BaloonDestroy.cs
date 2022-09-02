using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonDestroy : MonoBehaviour
{
    public float destroyPoint = 11.0f;
    void Update()
    {
        if(transform.position.y > destroyPoint){   
            Destroy(this.gameObject);
           // Debug.Log("Destroy!");
        }
    }
}
