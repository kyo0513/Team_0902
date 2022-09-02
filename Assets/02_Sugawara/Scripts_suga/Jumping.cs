using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float jumpForce;
    public bool isGrounded = false;
    public Animator animator;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();   
    }

    void Update()
    {
        if(isGrounded)
        {
            this.rb2D.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);    
            isGrounded = false;
            animator.SetTrigger("jump");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        isGrounded = true;
    }
}
