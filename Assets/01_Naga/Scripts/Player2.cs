using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    [Header("天井判定")]public GroundCheck head;
    [Header("接地判定")]public GroundCheck ground;
    [Header("踏みつけ判定の高さの割合(%)")] public float stepOnRate  = 10;
    [Header("移動速度")] [SerializeField] private int moveSpeed    = 3;
    [Header("ジャンプ力")][SerializeField] private int jumpForce   = 8;
    //音関連
    [Header("ジャンプする時に鳴らすSE")] public AudioClip jumpSE;
    [Header("やられた鳴らすSE")] public AudioClip downSE;

     //プライベート変数
    private Animator anim      = null;
    private Rigidbody2D rb     = null;
    private CapsuleCollider2D capcol  = null;
    private SpriteRenderer sr  = null;
    private MoveFloor moveObj  = null;    //移動床用 08/28
    private bool isGround      = false;
    //private bool isJump        = false;
    private bool isRun         = false;
    //private bool isHead        = false; //ヘッド判定　一旦使用しない　09/03    
    private bool isDown        = false;
    private bool isOtherJump   = false;
    private bool isContinue    = false;
    private bool isClearMotion = false;
    private bool isFalling     = false;  //ジャンプ改善用 09/02
    
    ////  はしご処理追加　09/03  ///////
    private bool isCliming     = false;
    private bool ladder_sin    = false;
    ///////////////////////////////////

    private float jumpPos         = 0.0f;
    private float otherJumpHeight = 0.0f;
    private float otherJumpSpeed  = 0.0f;
    private float beforeKey       = 0.0f;
    private float continueTime    = 0.0f;
    private float blinkTime       = 0.0f;

    //ゲームオーバー処理追加 08/27
    private bool nonDownAnim    = false;

    //タグ関連置き場
    //落下エリア・ダメージエリア追加 08/27
    private string groundTag    = "Ground";
    private string enemyTag     = "Enemy";
    private string deadAreaTag  = "DeadArea";
    private string hitAreaTag   = "HitArea";
    private string moveFloorTag = "Move";
    private string fallFloorTag = "Fall";
    private string jumpStepTag  = "JumpStep";

    //改修用
    private bool isMoving  = false;     //移動中判定    
    private bool isJumping = false;     //ジャンプ中判定用


    void Start()
    {
        //コンポーネントのインスタンスを捕まえる
        anim   = GetComponent<Animator>();
        rb     = GetComponent<Rigidbody2D>();
        capcol = GetComponent<CapsuleCollider2D>();
        sr     = GetComponent<SpriteRenderer>();
    }
    private void Update() 
    {
        if (isContinue)
        {
            //明滅　ついている時に戻る
            if (blinkTime > 0.2f)
            {
                sr.enabled = true;
                blinkTime = 0.0f;
            }
            //明滅　消えているとき
            else if (blinkTime > 0.1f)
            {
                sr.enabled = false;
            }
            //明滅　ついているとき
            else
            {
                sr.enabled = true;
            }
            //1秒たったら明滅終わり
            if (continueTime > 1.0f)
            {
                isContinue   = false;
                blinkTime    = 0.0f;
                continueTime = 0.0f;
                sr.enabled   = true;
            }
            else
            {
                blinkTime    += Time.deltaTime;
                continueTime += Time.deltaTime;
            }
        }

        if (!isDown && !GameController.instance.isGameOver && !GameController.instance.isStageClear)
        {            
            //接地判定を得る
            isGround = ground.IsGround();
            //isHead   = head.IsGround();  一旦使用しない 09/03

            //各種座標軸の速度を求める
            float xSpeed = GetXSpeed();
            float ySpeed = GetYSpeed();

            float horizontal = Input.GetAxis("Horizontal");     //移動量
            isMoving  = horizontal != 0;                        //移動中
        
            //向き処理下がった場合はアニメーション反転
            if (isMoving)
            {
                Vector3 scale = gameObject.transform.localScale;
                //右向きから左向きになった時、その反対の時下記に一致する
                if(horizontal < 0 && scale.x > 0 || horizontal > 0 && scale.x < 0)
                {
                    scale.x *= -1;
                }
                gameObject.transform.localScale = scale;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isFalling)
            {
                isCliming = false;
                moveObj   = null;
                Jump();
            }

            //アニメーションを適用
            SetAnimation();            
            
            //移動床処理追加 08/28            
            Vector2 addVelocity = Vector2.zero;            
            if (moveObj != null)
            {
                //Debug.Log("乗ってる処理");
                isFalling   = false;
                addVelocity = moveObj.GetVelocity();
                rb.velocity = new Vector2(xSpeed, ySpeed) + addVelocity;
            }else
            {
                isFalling = rb.velocity.y < -0.5f;             
                rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = new Vector2(0,rb.velocity.y);
            isFalling   = false;
            isJumping   = false;

            if (!isClearMotion && GameController.instance.isStageClear)
            {
                //anim.Play("player_clear");
                isClearMotion = true;
            }
            //rb.velocity = new Vector2(0, -gravity);
        }
        
        float y = Input.GetAxisRaw("Vertical");   //方向キーのy方向の入力

        if(ladder_sin)
        {            
            if(y>0)
            {
                isCliming   = true;  //仮設定　登る絵に遷移する条件がないので絵が変わらない
                isJumping   = false;
                isOtherJump = false;
                isFalling   = false;
            }
        }
        else        
        {
            isCliming = false; 
        }

        if(isCliming)
        {            
            rb.velocity = new Vector2(rb.velocity.x, y*moveSpeed);  //上にのぼる
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1;
        }
    } 


    void Jump()
    {        
        isJumping = true;                                          //ジャンプ判定へ        
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);  //ジャンプ力に力を加える
    }

      void otherJump()
    {
        isOtherJump = true;
        rb.AddForce(Vector2.up * otherJumpHeight, ForceMode2D.Impulse);
    }
    
    private float GetYSpeed()   /// Y軸速度計算用
    {
        //float verticalKey = Input.GetAxis("Vertical");
        //float ySpeed = -gravity;
        float ySpeed = 0;
        return ySpeed;
    }

    private float GetXSpeed()   /// X軸速度計算用
    {
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed        = 0.0f;

        if (horizontalKey > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isRun      = true;
            xSpeed     = moveSpeed;
        }
        else if (horizontalKey < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isRun      = true;
            xSpeed     = -moveSpeed;
        }
        else
        {
            isRun    = false;
            xSpeed   = 0.0f;
        }

        beforeKey = horizontalKey;
        return xSpeed;
    }
    
    private void SetAnimation()  /// アニメーションを設定する
    {        
        anim.SetBool("IsJumping"  , isJumping || isOtherJump);
        anim.SetBool("IsMoving"   , isRun);
        anim.SetBool("IsFalling"  , isFalling);  //ジャンプ改善 09/02
        //物を踏んで跳ねた時のアニメーション条件を入れること       09/03
        anim.SetBool("IsClimb"    , isCliming);
    }

    //コンテニューとダウンアニメーション関連
    public bool IsContinueWaiting() 
    {
        //return IsDownAnimEnd();
        if (GameController.instance.isGameOver)
        {
            return false;
        }
        else
        {
            return IsDownAnimEnd() || nonDownAnim;
        } 
    }

    //ダウンアニメーション完了判定処理
    private bool IsDownAnimEnd() 
    {
        if(isDown && anim != null) 
        {
            AnimatorStateInfo currentState =
                anim.GetCurrentAnimatorStateInfo(0);

            if (currentState.IsName("Mouse_Down")) 
            //if (currentState.IsName("Play_Down"))
            {
                if(currentState.normalizedTime >= 1) 
                {
                    return true; 
                } 
            } 
        } 
        return false; 
    }

    /// コンティニューする
    public void ContinuePlayer()
    {
        //GameController.instance.PlaySE(continueSE);
        isDown        = false;
        //anim.Play("Play_Idle");
        anim.Play("Mouse_Idle");
        isJumping     = false;
        isOtherJump   = false;
        isRun         = false;
        isContinue    = true;
        nonDownAnim   = false;
    }

    //やられた時の処理
    private void ReceiveDamage(bool downAnim) 
    { 
        //if (isDown)
        if (isDown || GameController.instance.isStageClear)
        {     
            return;
        }
        else
        {
            if (downAnim)
            {
                //anim.Play("Play_Down");
                anim.Play("Mouse_Down");
            }
            else
            {
                nonDownAnim = true;
            }
            isDown = true;
            //GameController.instance.PlaySE(downSE);
            GameController.instance.Sublife();
        }
    }

    //接地判定関連
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool enemy     = (collision.collider.tag == enemyTag);
        bool moveFloor = (collision.collider.tag == moveFloorTag);
        bool fallFloor = (collision.collider.tag == fallFloorTag);
        bool jumpStep  = (collision.collider.tag == jumpStepTag);

        if (enemy || moveFloor || fallFloor || jumpStep)
        {
            //踏みつけ判定になる高さ
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));

            //踏みつけ判定のワールド座標
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

            foreach (ContactPoint2D p in collision.contacts)
            {
                if (p.point.y < judgePos)
                {
                    if (enemy || fallFloor || jumpStep)
                    {
                        Enemy1 o = collision.gameObject.GetComponent<Enemy1>();
                                        
                        if (o != null)
                        {
                            if (enemy || jumpStep)
                            {
                                otherJumpHeight = o.boundHeight;    //踏んづけたものから跳ねる高さを取得する
                                otherJumpSpeed  = o.jumpSpeed;
                                o.playerStepOn  = true;             //踏んづけたものに対して踏んづけた事を通知する
                                jumpPos = transform.position.y;     //ジャンプした位置を記録する
                                otherJump();
                            }
                            else if(fallFloor)
                            {
                                o.playerStepOn  = true;
			                }
                        }
                        else
                        {
                            Debug.Log("ObjectCollisionが付いてないよ!");
                        }
                    }
                    else if(moveFloor)
		            {
                        moveObj = collision.gameObject.GetComponent<MoveFloor>();
                    }
                }
                else
                {
                    if (enemy)
                    {
                        ReceiveDamage(true);
                        break;
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == moveFloorTag)     //動く床から離れた
        {
            moveObj = null;
        }        
    }

    //ダメージ床　落下判定 08/27
    private void OnTriggerEnter2D(Collider2D collision)	
    {
        //大幅改修用/////////////////////////////////////
        if (collision.CompareTag(groundTag) || collision.CompareTag(moveFloorTag) ||  collision.CompareTag(fallFloorTag))
        {
            isJumping   = false;
            isOtherJump = false;
            //isFalling   = false;    //試し追加 → 足元コライダーの設定次第では斜めの坂は足がついていない
        }
        ////////////////////////////////////////////////

        if(collision.tag == deadAreaTag)
	    {
            GameController.instance.Zerolife();
            //ReceiveDamage(false);
	    }
	    else if(collision.tag == hitAreaTag)
	    {
            ReceiveDamage(true);
	    }

        //はしご処理追加　09/03
        if (collision.CompareTag("Test1"))
        {
            ladder_sin = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
         //はしご処理追加　09/03
        if (collision.CompareTag("Test1"))
        {
            ladder_sin = false;
        }

    }


}