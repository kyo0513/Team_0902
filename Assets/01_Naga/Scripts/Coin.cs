using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("加算するスコア")]      public int mycoin;
    [Header("プレイヤーの判定")]    public PlayerTriggerCheck playerCheck;
    private AudioSource audioSource = null;
    [Header("プレイヤーゲームオブジェクト")] public Player2 playerObj;
    [Header("コイン取得時SE")]             public AudioClip coinSE;
    // Update is called once per frame
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }
    void Update()
    {
        //プレイヤーが判定内に入ったら
        if (playerCheck.isOn)
        {
            if(GameController.instance != null)
            {
                playerObj.PlaySE(coinSE);
                GameController.instance.coin += mycoin;
                Destroy(this.gameObject);
            }
        }
    }  
}
