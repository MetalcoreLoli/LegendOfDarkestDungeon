using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{

    public GameObject Cam;
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        int deltaX = (int)Input.GetAxis("Horizontal");
        int deltaY = (int)Input.GetAxis("Vertical");

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
            Invoke("Restart", 1);
        }
    }

    protected override void OnCantMove<T>(T comp)
    {
        throw new System.NotImplementedException();
    }
}
