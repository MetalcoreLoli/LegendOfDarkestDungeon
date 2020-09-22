using Assets.Scripts.Dices;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    private Animator animator;
    public int damage = 1;
    public bool isActive = true;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("playerOut");

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           // Debug.Log("Player In trap");
            animator.SetTrigger("playerStand");
            var player = collision.GetComponent<Player>();
            if (DiceManager.RollDice("1d20") > 5 + player.Characteristics.DexterityMod && isActive)
                player.LoseHp(DiceManager.RollDice("1d4"));
            //else
            //    TextPopUp.CreateWithColor(transform.position, "Miss", player.DamageDealer.Text.transform, Color.red);

            isActive = false;
        }
        if (collision.CompareTag("Enemy"))
        {
            //Debug.Log("Enemy In trap");
            var enemy = collision.GetComponent<Enemy>();
            //animator.SetTrigger("playerStand");
            if (DiceManager.RollDice("1d20") > 10 && isActive)
                enemy?.TakeDamage(DiceManager.RollDice("1d4"), false);
            //isActive = false;
        }
    }
}
