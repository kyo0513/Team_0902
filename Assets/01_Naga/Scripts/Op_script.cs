using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Op_script : MonoBehaviour
{
    // Start is called before the first frame update
    
    [Header("移動速度")] [SerializeField] private float speed  = 12.5f;
    void Start()
    {
        Invoke("Scene_move", speed);        
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
