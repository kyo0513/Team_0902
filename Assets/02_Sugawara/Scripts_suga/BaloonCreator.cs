using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonCreator : MonoBehaviour
{
    Rigidbody2D rb2D;
    public GameObject[] baloonPrefabs;
    public Transform baloonParent;
    public float baloonAreaStart;
    public float baloonAreaEnd;
    public float baloonCreatePositionY;
    private float upSpeedMin = 90;
    private float upSpeedMax = 200;
    public float start = 0.0f;
    public float interval = 5.0f;
    private float time = 0;


    void Start()
    {
    }
    void Update()
    {
        time += Time.deltaTime;
        float xPosition = Random.Range(baloonAreaStart,baloonAreaEnd);
        if(time >= interval){
            float upSpeed = Random.Range(upSpeedMin,upSpeedMax);
            GameObject baloon = (GameObject)Instantiate(
                SelectBaloon(),
                new Vector2(xPosition,baloonCreatePositionY),
                Quaternion.identity
            );
            rb2D = baloon.GetComponent<Rigidbody2D>();
            baloon.transform.parent = baloonParent;
            this.rb2D.AddForce(Vector2.up * upSpeed);
        time = 0;
        }
    }

    GameObject SelectBaloon()
    {
        int index = Random.Range(0,baloonPrefabs.Length);
        return baloonPrefabs[index];
    }
}
