using Assets.Scripts.Actors;
using Assets.Scripts.Dices;
using Assets.Scripts.Stats;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Enemy : MovingObject
{
	[SerializeField] private readonly DamageDealer damageDealer;

    public int Damage = DiceManager.RollDice("2d6");

	private Animator animator;                          
	private Transform target;                           
	private bool skipMove;

	public float MaxDistanceToPlayer = 5f;

	public ActorCharacteristics characteristics;
	public bool WasDamaged;
	private int damaged;
	private bool isDamagedByPlayer;

	private void Awake()
	{
		characteristics = new ActorCharacteristics(DiceManager.RollUndSumFromString("2d4"), DiceManager.RollUndSumFromString("2d4"));
	}


	private void Update()
	{
		if (characteristics.Hp <= 0)
		{
			if (isDamagedByPlayer)
				GameManager.Instance.Player.UpdateExp(DiceManager.RollUndSumFromString("2d4"));
            if (DiceManager.RollDice("1d20") > 6)
            {
                if (DiceManager.RollDice("1d20") > 10)
                    GameManager.Instance.itemManager.DropAt(transform.position, "ManaPotion");
                else 
                    GameManager.Instance.itemManager.DropAt(transform.position, "HealingPotion");
            
            }
			Destroy(gameObject);
			//gameObject.SetActive(false);	
		}
		else
		{
            var player = GameManager.Instance.Player;
            //float dis = (player.transform.position - transform.position).sqrMagnitude;
            //var end = (player.GetComponent<Rigidbody2D>().position - rb2D.position);

            if (transform.position == player.transform.position)
            {

                TakeDamage(characteristics.MaxHp * 2, true);
            }

            //if (dis <= MaxDistanceToPlayer && player.GetComponent<Rigidbody2D>().position != rb2D.position)
            //{
            //	Debug.DrawLine(rb2D.position, rb2D.position + end.normalized * end.magnitude, Color.white);
            //	MoveEnemy();
            //}
        }
	}

	protected override void Start()
	{
		///GameManager.Instance.Enemies.Add(this);
		animator	= GetComponent<Animator>();
		target		= GameManager.Instance.Player?.transform;
		base.Start();
	}
	
	public void MoveEnemy()
	{
		int xDir = 0;
		int yDir = 0;

		if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
			yDir = target.position.y > transform.position.y ? 1 : -1;
		else
			xDir = target.position.x > transform.position.x ? 1 : -1;
		
		//Debug.Log($"Moving to {xDir}{yDir}");
		AttemptMove<Player>(xDir, yDir);
	}
	
	protected override void AttemptMove<T>(int xDir, int yDir)
	{
		//if (skipMove)
		//{
		//	skipMove = false;
		//	return;
		//}
		base.AttemptMove<T>(xDir, yDir);

		//Now that Enemy has moved, set skipMove to true to skip next move.
		skipMove = true;
	}

	protected override void OnCantMove<T>(T component)
	{
		Player hitPlayer = component as Player;
		if (isActiveAndEnabled)
		if (DiceManager.RollDice("1d20") > 10 + hitPlayer.GetComponent<Actor>().Characteristics.DexterityMod)
			hitPlayer.LoseHp(Damage);
	}

	public void TakeDamage(int damage, bool byPlayer)
	{
		animator?.SetTrigger("TakeDamage");
		characteristics.Hp -= damage;
		WasDamaged = true;
		//TextPopUp.CreateAt(transform.position, damage, damageDealer.Text.transform);
		damaged = damage;
		isDamagedByPlayer = byPlayer;
	}

	private void OnDestroy()
	{

	}
}
