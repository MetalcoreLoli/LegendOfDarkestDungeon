using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    GameObject player;
    public float speed = 3.0f;
    public float fov = 60f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //float deltaX = Input.GetAxis("Horizontal")    * speed;
        //float deltaY = Input.GetAxis("Vertical")      * speed;

        if (player != null)
            transform.position = (new Vector3(player.transform.position.x, player.transform.position.y, -10));

        Camera.main.fieldOfView = fov;

        //transform.Translate(deltaX, deltaY, 0);
    }
}
