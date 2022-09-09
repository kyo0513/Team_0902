using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //カメラはネコゲームから流用
    public GameObject player;
    [Header("カメラ調整X")][SerializeField] private float x_hosei   = 0;
    [Header("カメラ調整Y")][SerializeField] private float y_hosei   = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos=player.transform.position;
        transform.position=new Vector3(
            //transform.position.x,
            player.transform.position.x + x_hosei,
            player.transform.position.y + y_hosei,
            transform.position.z
        );
        
    }
}
