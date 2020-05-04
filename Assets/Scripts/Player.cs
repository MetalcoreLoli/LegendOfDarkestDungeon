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

public enum LookDir
{ 
    Up      = 0,
    Down    = 1,
    Left    = 2,
    Right   = 3
}

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

    public Camera Camera;
    
    
    private bool isFlipLeftRight    = false;
    private bool isFlipUpDown       = false;

    public LookDir lookDir = LookDir.Left;

    Vector2 mousePosition;

    protected override void Start()
    {
        animator = GetComponent<Animator>();

        base.Start();
    }


    // Update is called once per frame
    void Update()
    {

        if (GameManager._instance.playersTurn == false) return;

        horizontal  = (Input.GetAxisRaw("Horizontal"));
        vertical    = (Input.GetAxisRaw("Vertical"));

        mousePosition = Camera.ScreenToWorldPoint(Input.mousePosition);

        if (horizontal != 0)
        {
            if (horizontal > 0)
                lookDir = LookDir.Right;
            else
                lookDir = LookDir.Left;

            if (horizontal > 0 && !isFlipLeftRight)
            {
                ///GameObject.FindGameObjectWithTag("PlayersLight").transform.Rotate(0.0f, -180.0f, 0.0f);
                FlipLeftRight();
            }
            if (horizontal < 0 && isFlipLeftRight)
            {
                FlipLeftRight();
            }
            vertical = 0;
        }
        if (vertical != 0)
        {
            if (vertical > 0)
                lookDir = LookDir.Up;
            else 
                lookDir = LookDir.Down;
            
            if (vertical > 0 && !isFlipUpDown)
            {
                FlipFirePointUpDown();
            }

            if (vertical < 0 && isFlipUpDown)
            {
                FlipFirePointUpDown();
            }

            //FliLeftRight();
            horizontal = 0;
        }

        

        //animator.SetFloat("Horizontal", horizontal);
        //animator.SetFloat("Vertical", vertical);
        //animator.SetFloat("speed", new Vector2(horizontal, vertical).sqrMagnitude);
    }

    private void FlipLeftRight()
    {
        isFlipLeftRight = !isFlipLeftRight;
        var vec = transform.localScale;
        vec.x *= -1;
        transform.localScale = vec;
        GameObject.FindGameObjectWithTag("PlayersFirePoint").transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void FlipFirePointUpDown()
    {
        isFlipUpDown = !isFlipUpDown;
        var firePoint = GameObject.FindGameObjectWithTag("PlayersFirePointUp");
        var vec = firePoint.transform.localPosition;
        vec.y *= -1;
        firePoint.transform.localPosition = vec;

        if (lookDir == LookDir.Down)
        { 
            var veca = firePoint.transform.localEulerAngles;
            veca.z = -90f;
            firePoint.transform.localEulerAngles = veca;
        }
        if (lookDir == LookDir.Up)
        {
            var veca = firePoint.transform.localEulerAngles;
            veca.z = 90f;
            firePoint.transform.localEulerAngles = veca;
        }



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

    public void PlayerCastSpell()
    { 
        animator.SetTrigger("Casting");
    }
    protected override void OnCantMove<T>(T comp)
    {
        throw new System.NotImplementedException();
    }
}
