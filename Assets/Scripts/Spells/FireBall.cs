using Assets.Scripts.Dices;
using Assets.Scripts.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Spell
{

    public float speed = 20f;

    public FireBall()
    { 
    
    }

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        rb2D.velocity = transform.right * speed;
        Debug.Log(Info);
    }

    private void FixedUpdate()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().TakeDamage(DiceManager.RollDice("1d4"), true);
        }
        Destroy(gameObject);
    }

    public override void Cast()
    {
        var firePointPos = FirePoint.position;
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Debug.Log(Info);
        if (player.PlayerCastSpell(this.Info.Cost))
        {
            if (player.lookDir == LookDir.Left || player.lookDir == LookDir.Right)
                Instantiate(Info.Prefab, FirePoint.position, FirePoint.rotation);
            else
            {
                Instantiate(Info.Prefab, FirePointUp.position, FirePointUp.rotation);
                firePointPos = FirePointUp.position;
            }

            rb2D.AddForce(firePointPos * 10.0f, ForceMode2D.Impulse);

        }
    }
       
}
