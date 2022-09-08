using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    public float openSpeed;
    public float maxPositionL;
    public float maxPositionR;
    private float startPositionLX;
    private float startPositionRX;
    private float startPositionLY;
    private float startPositionRY;
    private bool isEnter;
    Vector2 disL;
    Vector2 disR;
    Vector2 startPosL;
    Vector2 startPosR;

    void Start()
    {
        startPosL = leftDoor.transform.position;
        startPosR = rightDoor.transform.position;
        disL = new Vector2(startPosL.x-1,startPosL.y);
        disR = new Vector2(startPosR.x+1,startPosR.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isEnter = true;
    }
    //L20.36
    //R21.7

    void Update()
    {
        if(isEnter)
        {
         leftDoor.transform.position = Vector2.Lerp(leftDoor.transform.position,disL,openSpeed*Time.deltaTime);   
         rightDoor.transform.position = Vector2.Lerp(rightDoor.transform.position,disR,openSpeed*Time.deltaTime);   
        }
        else
        {
         leftDoor.transform.position = Vector2.Lerp(leftDoor.transform.position,startPosL,openSpeed*Time.deltaTime);   
         rightDoor.transform.position = Vector2.Lerp(rightDoor.transform.position,startPosR,openSpeed*Time.deltaTime);   
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isEnter = false;
    }
}
