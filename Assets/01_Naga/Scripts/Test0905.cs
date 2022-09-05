using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test0905 : MonoBehaviour
{
    [Header("TestCanvas")]  public GameObject testcanvas;

    // Start is called before the first frame update
    void Start()
    {
        testcanvas.SetActive(false);
        Invoke(nameof(TestMethod), 10.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TestMethod()
    {
        testcanvas.SetActive(true);
        
    }
}
