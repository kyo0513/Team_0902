using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_zako1 : MonoBehaviour
{
    private SpriteRenderer sr   = null;
    [Header("移動速度")]public float speed;
    [Header("重力")]public float gravity;     
    [Header("画面外でも行動する")] public bool nonVisibleAct;  //インスペクターから設定する
    [Header("右向きスタート")]     public bool rightstart;     //インスペクターから設定する
    [Header("加算スコア")]         public int myScore;
    private AudioSource audioSource = null; 
    [Header("踏まれた時のSE")]     public AudioClip stepSE;

    private Rigidbody2D  rb     = null;
    private bool rightTleftF    = false;
    private Enemy1 oc           = null;
    private BoxCollider2D col   = null;
    private bool isDead         = false;
    [HideInInspector]public bool isOn = false;               //敵か壁にあたると反転する用

    // Start is called before the first frame update
    void Start()
    {        
        rb  = GetComponent<Rigidbody2D>();
        sr  = GetComponent<SpriteRenderer>(); 
        oc  = GetComponent<Enemy1>();
        col = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        if(rightstart)
        {
            //Debug.Log("右向きON");
            rightTleftF = true;
        }   
    }

    // Update is called once per frame
    void FixedUpdate()    
    {
        if (!oc.playerStepOn)
        {   
            if (sr.isVisible || nonVisibleAct)     //カメラ内にいるか画面外行動がONか
            {      
                //Debug.Log(rightTleftF);      
                //Debug.Log("画面に見えている");
                if (isOn)                          //壁か敵にぶつかった場合反対方向に歩く
                {
                    rightTleftF = !rightTleftF;
                }

                //int xVector = -1;
                int xVector;
                if (rightTleftF)
                {
                    xVector = 1;
                    if(rightstart)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    
                }
                else
                {
                    xVector = -1;
                    if(rightstart)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    
                }

                rb.velocity = new Vector2(xVector * speed, -gravity);
            }
            else
            {
                rb.Sleep();
            }
        }
        else
        {
            if (!isDead)
            {
                audioSource.PlayOneShot(stepSE);

                if(GameController.instance != null)
                {
                    GameController.instance.coin += myScore;
                }
                
                //anim.Play("dead");
                rb.velocity = new Vector2(0, -gravity); 
                isDead = true;
                col.enabled = false;
                Destroy(gameObject,3f);
            }
            else
            {
                transform.Rotate(new Vector3(0,0,5));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.tag == "Enemy")
        {
            isOn = true;
        }

        if (collision.tag == "DeadArea" )
        {
            Destroy(gameObject,2f);
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.tag == "Enemy")
        {
            isOn = false;
            //isOn = !isOn;
        }
    }



}