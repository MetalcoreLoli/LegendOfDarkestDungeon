using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{

    public float speed = 5f;

    private float restartLevelDelay = 1f;
    private Animator animator;

    private float horizontal  = 0;
    private float vertical    = 0;



    protected override void Start()
    {
        animator = GetComponent<Animator>();


        base.Start();
    }


    // Update is called once per frame
    void Update()
    {

        if (GameManager._instance.playersTurn == false) return;

        horizontal = (Input.GetAxisRaw("Horizontal"));
        vertical = (Input.GetAxisRaw("Vertical"));
    
        if (horizontal != 0)
            vertical = 0;
        else if (vertical != 0)
            horizontal = 0;
        //animator.SetFloat("Horizontal", horizontal);
        //animator.SetFloat("Vertical", vertical);
        //animator.SetFloat("speed", new Vector2(horizontal, vertical).sqrMagnitude);
    }

    private void FixedUpdate()
    {
        // rb2D.MovePosition(rb2D.position + new Vector2(horizontal, vertical) * speed * Time.fixedDeltaTime);

        if (horizontal != 0 || vertical != 0)
        { 
            AttemptMove<Wall>((int)(horizontal), (int)(vertical));
            //IsMoving = false;
        }

    }

   
    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Exit"))
        {
            Invoke("Restart", restartLevelDelay);
            GameManager._instance.enabled = false;
        }

        if (collision.tag == "Trap")
        { 
            animator.SetTrigger("Hit");
        }
    }




    protected override void OnCantMove<T>(T comp)
    {
        throw new System.NotImplementedException();
    }
}
