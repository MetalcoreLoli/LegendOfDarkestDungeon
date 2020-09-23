using Assets.Scripts.Actors;
using Assets.Scripts.Dices;
using Assets.Scripts.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Spell
{

    public float speed = 20f;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.tag == "Enemy")
        {
            var enemy = collision.GetComponent<Enemy>();
            enemy?.TakeDamage(DiceManager.RollDice("1d4"), true);
        }
        Destroy(gameObject);
    }

    public override void Cast(Transform caster)
    {
        var firePointPos = caster.position;
        var caster1 = caster.GetComponent<Player>();
        Debug.Log(Info.Name + $" was cast by {caster1.GetType().Name}");
        if (caster1.PlayerCastSpell(Info.Cost))
        {
            if (caster1.lookDir == LookDir.Left || caster1.lookDir == LookDir.Right)
            {
                firePointPos += Vector3.left;
                Instantiate(Info.Prefab, firePointPos, FirePoint.rotation);
            }
            else
            {
                firePointPos += Vector3.up;
                Instantiate(Info.Prefab, firePointPos, FirePointUp.rotation);
            }
            rb2D.AddForce(firePointPos * 10.0f, ForceMode2D.Impulse);
        }
    }
       
}
