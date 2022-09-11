using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//音楽素材　ポケットサウンド様の素材を使用させて頂いております。下記URL
//https://pocket-se.info/rules/

public class StageCtrl : MonoBehaviour
{
    [Header("プレイヤーゲームオブジェクト")] public GameObject playerObj;
    //[Header("コンティニュー位置")] public GameObject[] continuePoint;
    
    //ゲームオーバー・クリア関連
    [Header("ゲームオーバー")]  public GameObject gameOverObj;
    [Header("フェード")]       public FadeImage fade;
    [Header("ゲームオーバー時に鳴らすSE")] public AudioClip gameOverSE;
    [Header("リトライ時に鳴らすSE")] public AudioClip retrySE;
    [Header("ステージクリアーSE")]   public AudioClip stageClearSE;
    [Header("ステージクリア")]       public GameObject stageClearObj;
    [Header("ステージクリア判定")]   public PlayerTriggerCheck stageClearTrigger;

    private Player2 p;             //Player2に変更　09/03

    //ゲームオーバー&リトライ処理
    private AudioSource audioSource = null;
    private int nextStageNum;

    [SerializeField] private string retrystage = "";   //お試しシーン遷移用追加 08/28
    [SerializeField] private string nextstage  = "";   //お試しシーン遷移用追加 08/28
    private string movestage;

    private bool startFade     = false;
    private bool doGameOver    = false;
    private bool retryGame     = false;
    private bool doSceneChange = false;
    private bool doClear       = false;
 
    void Start()
    {
        //Debug.Log("ステージコントローラー　スタート処理");
        audioSource = GetComponent<AudioSource>();

        //if (playerObj != null && continuePoint != null && continuePoint.Length > 0)
        //if (playerObj != null && continuePoint != null && continuePoint.Length > 0 && gameOverObj != null && fade != null && stageClearObj != null) //コンテニュー時　戻り削除09/02
        if (playerObj != null && gameOverObj != null && fade != null && stageClearObj != null)
        {
            gameOverObj.SetActive(false);  //ゲームオーバーは初期はオフ
            stageClearObj.SetActive(false);
            //playerObj.transform.position = continuePoint[0].transform.position; //コンテニュー位置削除 09/02
            p = playerObj.GetComponent<Player2>();
            if(p == null)
            {
                Debug.Log("プレイヤーじゃない物がアタッチされているよ！");
            }
        }
        else
        {
        Debug.Log("設定が足りてないよ！");
        }
    }
 
    void Update()
    {
        //ゲームオーバー時
        if (GameController.instance.isGameOver && !doGameOver)
        {
            gameOverObj.SetActive(true);
            audioSource.Stop();
            audioSource.PlayOneShot(gameOverSE);
            doGameOver = true;
        }
        //プレイヤーがやられた時の処理
        else if (p != null && p.IsContinueWaiting() && !doGameOver)
        {   
            //コンテニューポイント処理削除
            /* 
            if(continuePoint.Length > GameController.instance.continueNum)
            {
                playerObj.transform.position = 
                continuePoint[GameController.instance.continueNum].transform.position;
                p.ContinuePlayer();
            }
            else
            {
                Debug.Log("コンティニューポイントの設定が足りてないよ！");
            }
            */
            p.ContinuePlayer();
        }
        //ステージクリア処理追加 08/28
        else if (stageClearTrigger != null && stageClearTrigger.isOn && !doGameOver && !doClear)
        {
            StageClear();
            doClear = true;
        }

        //ステージを切り替える
        if (fade != null && startFade && !doSceneChange)
        {
            if (fade.IsFadeOutComplete())
            {
                //ゲームリトライ
                if (retryGame)
                {
                    GameController.instance.RetryGame();
                    movestage = retrystage;
                }
                //次のステージ
                else
                {
                    GameController.instance.stageNum = nextStageNum;
                    movestage = nextstage;
                }
                GameController.instance.isStageClear = false;
                Invoke("Scene_move", 1.0f);
                //SceneManager.LoadScene(movestage);
                //doSceneChange = true;
            }
        }
    }

    /// </summary>
    public void Retry()
    {        
        audioSource.PlayOneShot(retrySE);
        //GameController.instance.PlaySE(retrySE);
        ChangeScene(1);      //最初のステージに戻るので１
        retryGame = true;
    }
    // ステージを切り替えます。
    /// <param name="num">ステージ番号</param>
    public void ChangeScene(int num)
    {
        if (fade != null)
        {
            nextStageNum = num;
            fade.StartFadeOut();
            startFade = true;
        }
    }

    public void StageClear()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(stageClearSE);
        GameController.instance.isStageClear = true;
        stageClearObj.SetActive(true);
        //GameController.instance.PlaySE(stageClearSE);
    }

    public void Scene_move()
    {
        doSceneChange = true;
        SceneManager.LoadScene(movestage);
    } 


  
}
