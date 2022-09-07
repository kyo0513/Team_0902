using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))] //アニメーションで追加

public class SpireSpiderController : MonoBehaviour
{
    [SerializeField]private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            animator.SetBool("IsApproach", true);
            animator.SetBool("IsHyde",  true );
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            animator.SetBool("IsApproach", true);
            animator.SetBool("IsHyde",  true );
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            animator.SetBool("IsApproach",false);
            animator.SetBool("IsHyde",  false );
        }
    }
}