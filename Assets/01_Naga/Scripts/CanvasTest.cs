using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasTest : MonoBehaviour
{
    public GameObject Canvas1;

    [SerializeField] private string nextStage = "";



    // Start is called before the first frame update
    void Start(){     
    }

    // Update is called once per frame
    void Update(){        
    }

    private void OnTriggerEnter2D(Collider2D collision)	
    {
        Canvas1.SetActive (true);        
    }


    private void OnTriggerExit2D(Collider2D collision)	
    {
        Canvas1.SetActive (false); 
    }

    public void Click_Yes()
    {
        SceneManager.LoadScene(nextStage);

    }
    public void Click_No()
    {
        Canvas1.SetActive (false);

    }

    

    

    


    


}
