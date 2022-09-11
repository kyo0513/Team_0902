using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Op_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Scene_move", 12.5f);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Scene_move()
    {   
        SceneManager.LoadScene("StageSelection");   
        
    }


}
