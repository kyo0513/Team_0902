using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasTest : MonoBehaviour
{
    public GameObject Canvas1;
    private AudioSource audioSource = null; 
    [Header("Yes時に鳴らすSE")] public AudioClip yesSE;
    [Header("No時に鳴らすSE")]  public AudioClip noSE;

    [SerializeField] private string nextStage = "";



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Canvas1.SetActive (false);
    }

    // Update is called once per frame
    void Update(){        
    }

    private void OnTriggerEnter2D(Collider2D collision)	
    {
        //Debug.Log("キャンバス表示");
        Canvas1.SetActive (true);        
    }


    private void OnTriggerExit2D(Collider2D collision)	
    {
        //Debug.Log("キャンバス非表示");
        Canvas1.SetActive (false); 
    }

    public void Click_Yes()
    {   
        PlaySE(yesSE);
        Invoke("Scene_move", 1.0f);
    }
    public void Click_No()
    {
        //Debug.Log("押された");        
        PlaySE(noSE);
        Canvas1.SetActive (false);

    }

    public void PlaySE(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("オーディオソースが設定されていません");
        }
    }

    public void Scene_move()
    {   
        SceneManager.LoadScene(nextStage);   
        
    }    

    

    

    


    


}
