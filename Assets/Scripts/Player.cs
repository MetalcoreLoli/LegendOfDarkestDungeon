using Assets.Scripts.Dices;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{

    public float speed = 5f;

    private float restartLevelDelay = 1f;
    private Animator animator;

    private float horizontal  = 0;
    private float vertical    = 0;

    public int MaxHp;
    public int Hp;

    public int Mana;
    public int MaxMana;

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
        {
            if (horizontal > 0)
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            else
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

            vertical = 0;
        }
        else if (vertical != 0)
        { 
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            horizontal = 0;
        }

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Exit"))
        {
            Invoke("Restart", restartLevelDelay);
            GameManager._instance.enabled = false;
        }
    }

    public void LoseHp(int damage)
    {
        Hp -= damage;
        GameManager._instance.playersHp = Hp;
        animator.SetTrigger("Hit");


        var light = GameObject.FindGameObjectWithTag("PlayersLight").GetComponent<Light>();

        //float percent = (float)Hp / (float)MaxHp;

        light.intensity -= damage /*light.intensity * percent*/;
        
        if (Hp <= 0)
            GameManager._instance.GameOver();
    }


    protected override void OnCantMove<T>(T comp)
    {
        throw new System.NotImplementedException();
    }
}
