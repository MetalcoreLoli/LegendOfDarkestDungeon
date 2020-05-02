using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{


    private float restartLevelDelay = 1f;
    private Animator animator;

    protected override void Start()
    {
        animator = GetComponent<Animator>();


        base.Start();
    }

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager._instance.playersTurn == false) return;

        int deltaX =0;
        int deltaY = 0;

        deltaX = (int)(Input.GetAxisRaw("Horizontal"));
        deltaY = (int)(Input.GetAxisRaw("Vertical"));


        if (deltaX != 0)
            deltaY = 0;

        if (deltaX != 0 || deltaY != 0)
            AttemptMove<Wall>(deltaX, deltaY);

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
    }




    protected override void OnCantMove<T>(T comp)
    {
        throw new System.NotImplementedException();
    }
}
