using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalClacker : MonoBehaviour
{
    public GameObject[] clackersPrefabs;
    public Transform clackerParent;
    public const int MAXOBJECT = 100;
    public const int STARINDEX = 0;
    public const int SANKAKUINDEX = 1;
    public const int GOKAKUKEIINDEX = 2;
    public const int KURUKURUINDEX = 3;
    /*
    public int star = 25;
    public int sankaku = 50;
    public int gokakukei = 75;
    public int kurukuru = 10;
    */
    public int offset =1;
    public List<GameObject> clackerList = new List<GameObject>();
    public float deg = 360.0f/MAXOBJECT;
    public float minPower = 20.0f;
    public float maxPower = 60.0f;
    public float torque = 1.0f;
    public float rad;
    public bool isGoal = false;

    void Start()
    {
       Debug.Log(deg); 
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isGoal = true;
        rad = deg * Mathf.Deg2Rad;
        if(other.gameObject.tag == "Player")
        {
            //Debug.Log("ENTER");
            for(int i=0 ; i<MAXOBJECT ; i++)
            {
                if(i%2 == 0){
                    float rotate = Random.Range(0.0f,360.0f);
                    int index = Random.Range(0,4);
                    float xposition = transform.position.x + offset * Mathf.Cos(rad*i);
                    float yposition = transform.position.y + offset * Mathf.Sin(rad*i);
                    GameObject newClacker = (GameObject)Instantiate(
                        clackersPrefabs[index],
                        new Vector2(xposition,yposition),
                        Quaternion.identity
                    );
                    newClacker.transform.Rotate(0,0,rotate);
                    newClacker.transform.parent = clackerParent;
                    Rigidbody2D rb2D = newClacker.GetComponent<Rigidbody2D>();
                    float power = Random.Range(minPower,maxPower);
                    rb2D.AddForce(new Vector2(xposition - transform.position.x , yposition - transform.position.y) * power);
                    newClacker.transform.Rotate(0,0,torque);
                }
            }
        }
    }
}
