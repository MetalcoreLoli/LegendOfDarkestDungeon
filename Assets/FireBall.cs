using Assets.Scripts.Dices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    public float speed = 20f;

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().Hp -= DiceManager.FourEdges.Roll();
        }
        Destroy(gameObject);
    }

}
