using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal")  * speed;
        float deltaY = Input.GetAxis("Vertical") * speed;

        transform.Translate(deltaX, deltaY, 0);
    }
}
