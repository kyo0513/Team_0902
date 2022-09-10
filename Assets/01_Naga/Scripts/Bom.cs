using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bom : MonoBehaviour
{
    [Header("破壊対象")][SerializeField] private GameObject target;
    [Header("メインカメラ")][SerializeField] private CameraController main;
    [Header("爆発音SE")] public AudioClip bomSE;
    [Header("プレイヤーゲームオブジェクト")] public Player2 playerObj;
    [SerializeField] private Shake shake;
    private CapsuleCollider2D capcol  = null;
    private bool isAction        = false;


    // Start is called before the first frame update
    void Start()
    {
        capcol = GetComponent<CapsuleCollider2D>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)	
    {
        if(!isAction)
        {
            //GetComponent<CameraController>().enabled = true;
            main.enabled  = false;
            var duration  = 0.20f;
            var strength  = 03.0f;
            var vibrato   = 03.0f;
            shake.StartShake(duration, strength, vibrato);
            playerObj.PlaySE(bomSE);
            Destroy(target.gameObject);
            Invoke("Action", 0.3f);            
            isAction       = true;
            capcol.enabled = false;
        }  
    }

    public void Action()
    {   
        main.enabled  = true;
        Destroy(this.gameObject);  
        
    }


}
