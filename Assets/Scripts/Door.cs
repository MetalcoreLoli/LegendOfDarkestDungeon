using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public LayerMask BeforeEnter;
    public LayerMask AfterEnter;


    public Animator animator;



    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsClosed", false);
       // GetComponent<BoxCollider2D>().enabled = false;

    }
    // Update is called once per frame
    void Update()
    {
       // GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //gameObject.layer = AfterEnter;

        if (collision.tag == "Player")
        { 
            animator.SetBool("IsClosed", true);
        }

        //GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetBool("IsClosed", false);
        }
    }
}
