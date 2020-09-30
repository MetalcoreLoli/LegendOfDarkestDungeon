using Assets.Scripts.Actors;
using Assets.Scripts.Dices;
using Assets.Scripts.Spells;
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
        LookDir lookDir = caster.GetComponent<Player>().lookDir;
        var actor = caster.GetComponent<Actor>();
        Debug.Log(Info.Name + $" was cast by {0}");

        switch (lookDir)
        {
            case LookDir.Left:
                firePointPos += Vector3.left;
                break;

            case LookDir.Right:
                firePointPos += Vector3.right;
                break;

            case LookDir.Down:
                firePointPos += Vector3.down;
                break;

            case LookDir.Up:
                firePointPos += Vector3.up;
                break;

            default:
                break;
        }

        Instantiate(Info.Prefab, firePointPos, FirePoint.rotation);
        rb2D.AddForce(firePointPos * 10.0f, ForceMode2D.Impulse);
        actor.UpdateMana(-Info.Cost);
    }
}