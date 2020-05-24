using Assets.Scripts.Dices;
using Assets.Scripts.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidSpell : Spell
{
    
    public override void Cast()
    {
        var firePointPos = FirePoint.position;
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //Debug.Log(Info);
        if (player.PlayerCastSpell(Info.Cost))
        {
            if (player.lookDir == LookDir.Left || player.lookDir == LookDir.Right)
                Instantiate(Info.Prefab, FirePoint.position, FirePoint.rotation);
            else
            {
                Instantiate(Info.Prefab, FirePointUp.position, FirePointUp.rotation);
                firePointPos = FirePointUp.position;
            }

            //rb2D.AddForce(firePointPos * 10.0f, ForceMode2D.Impulse);

        }
        //Instantiate(Info.Prefab, player.transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Enemy In trap");
            var enemy = collision.GetComponent<Enemy>();
            if (DiceManager.RollDice("1d20") > 10)
                enemy.TakeDamage(DiceManager.RollDice("2d4"));

            StartCoroutine(DestoryAfter(1.5f));
            Destroy(gameObject);
        }

        StartCoroutine(DestoryAfter(4.5f));
    }

    IEnumerator DestoryAfter(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Destroy(gameObject);
    }
}
