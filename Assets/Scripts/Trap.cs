using Assets.Scripts.Dices;
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
        if (collision.tag == ("Player"))
        {
            animator.SetTrigger("playerStand");
            var player = collision.GetComponent<Player>();
            if (DiceManager.TwentyEdges.Roll() > 5 && isActive)
                player.LoseHp(Dice.Parser("1d4").Roll());

            isActive = false;
        }
    }
}
