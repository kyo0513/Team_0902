using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//コメント//
//ゲームコントローラーからパネル更新全般をこちらに移動 08/28

public class LifePanel : MonoBehaviour
{
    public GameObject[] icons;
    [Header("ゲームコントローラーを設定")] public GameController   gameController;
    [Header("ライフパネルを設定")]   public LifePanel  lifepanel;
    [Header("タイマーパネルを設定")] public Text       timepanel;
    [Header("コインテキストを設定")] public Text       cointext;
    [Header("トータルスコアを設定")] public Text       totaltext;

    public int score_now = 0;
    public int max_score;
    public int total_score = 0;
    //時間表示関連
    private float second;
    private int   minute;
    private int   hour;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GetComponent<GameController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameController == null)
        {   
            //Debug.Log("通過");
            gameController = GameController.instance;

        }

        UpdateLife((gameController.Life()));   //08/28

        if(timepanel != null)
        {
            //経過時間
            second += Time.deltaTime;

            if(minute > 60)
            {
                hour++;
                minute = 0;
            }
            if(second > 60f)
            {
                minute += 1;
                second = 0;
            }

            //timepanel.text = "" + Time.time;
            timepanel.text   = hour.ToString() + ":" + minute.ToString("00") + ":" + second.ToString("f2").PadLeft(5, '0');

        }
        //コイン取得
        //cointext.text    = "×" + coin.ToString("000");
        //cointext.text = "x" + gameController.Coin().ToString("000");
        //cointext.text = "Score: " + gameController.Coin().ToString("00000");
        max_score = gameController.Coin();
        if(score_now < max_score)
        {
            score_now += 10;
            cointext.text = "Score: " + score_now.ToString("00000");
        }

        //トータルスコア処理追加　09/09
        if(totaltext != null)
        {
            total_score    =  gameController.Total_Coin();
            totaltext.text =  "Total:" + total_score.ToString("000000");
        }

        
    }

    public void UpdateLife(int life){
        //Debug.Log("ライフ処理");
        for(int i = 0; i< icons.Length;i++){
            if(i < life) icons[i].SetActive(true);
            else icons[i].SetActive(false);
        }
    }
}
