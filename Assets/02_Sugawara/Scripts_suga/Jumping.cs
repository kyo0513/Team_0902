using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{
    [Header("ジャンプの高さ")]public float jumpForce;
    private bool isGrounded = false;
    public Animator animator;

    private SpriteRenderer sr   = null;
    [Header("移動速度")]public float speed;
    [Header("重力")]public float gravity;     
    [Header("画面外でも行動する")] public bool nonVisibleAct;  //インスペクターから設定する
    [Header("加算スコア")]         public int myScore;
    private AudioSource audioSource = null; 
    [Header("踏まれた時のSE")]     public AudioClip stepSE;

    private Rigidbody2D  rb     = null;
    private Enemy1 oc           = null;
    private BoxCollider2D col   = null;
    private bool isDead         = false;
    [HideInInspector]public bool isOn = false;               //敵か壁にあたると反転する用

    void Start()
    {        
        animator = GetComponent<Animator>();   
        rb  = GetComponent<Rigidbody2D>();
        sr  = GetComponent<SpriteRenderer>(); 
        oc  = GetComponent<Enemy1>();
        col = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

//JumpingProtType
    void OnCollisionEnter2D(Collision2D other)
    {
        isGrounded = true;
    }

    void FixedUpdate()    
    {
        if (!oc.playerStepOn)
        {   
            if (sr.isVisible || nonVisibleAct)     //カメラ内にいるか画面外行動がONか
            {
            //Jump
                if(isGrounded)
                {
                    this.rb.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);    
                    isGrounded = false;
                    animator.SetTrigger("jump");
                }
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
            if(collision.tag == "Enemy"){
                Debug.Log(collision.gameObject.name);
            }
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
