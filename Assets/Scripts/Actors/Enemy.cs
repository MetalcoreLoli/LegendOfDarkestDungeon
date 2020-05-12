﻿using Assets.Scripts.Dices;
using Assets.Scripts.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{

    public int Hp;

    public int Damage = DiceManager.RollDice("2d6");

	private Animator animator;                          
	private Transform target;                           
	private bool skipMove;

	public float MaxDistanceToPlayer = 5f;

	public ActorCharacteristics characteristics;

	private void Awake()
	{
		characteristics = new ActorCharacteristics(DiceManager.RollUndSumFromString("2d4"), DiceManager.RollUndSumFromString("2d4"));
		Hp				= characteristics.Hp;
	}

	private void FixedUpdate()
	{
		
	}

	private void Update()
	{
		Hp = characteristics.Hp;
		if (characteristics.Hp <= 0)
		{
			GameManager._instance.RemoveEnemy(this);
			gameObject.SetActive(false);	
		}
		else
		{
			var player = GameObject.FindGameObjectWithTag("Player");
			float dis = (player.transform.position - transform.position).sqrMagnitude;
			var end = (player.GetComponent<Rigidbody2D>().position - rb2D.position);
			
			if (transform.position == player.transform.position)
			{
				//GameManager._instance.Enemies.Remove(this);
				//Destroy(gameObject);

			}
			
			if (dis <= MaxDistanceToPlayer && player.GetComponent<Rigidbody2D>().position != rb2D.position)
			{
				Debug.DrawLine(rb2D.position, rb2D.position + end.normalized * end.magnitude, Color.white);
				MoveEnemy();
			}
		}
	}

	protected override void Start()
	{
		GameManager._instance.Enemies.Add(this);
		animator	= GetComponent<Animator>();
		target		= GameObject.FindGameObjectWithTag("Player").transform;
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
		if (skipMove)
		{
			skipMove = false;
			return;
		}
		base.AttemptMove<T>(xDir, yDir);

		//Now that Enemy has moved, set skipMove to true to skip next move.
		skipMove = true;
	}

	protected override void OnCantMove<T>(T component)
	{
		Player hitPlayer = component as Player;
		if (DiceManager.RollDice("1d20") > 10 + hitPlayer.Characteristics.DexterityMod)
			hitPlayer.LoseHp(Damage);

	}

	public void TakeDamage(int damage)
	{
		animator.SetTrigger("TakeDamage");
		characteristics.Hp -= damage;
	}

	private void OnDestroy()
	{
		Destroy(this);
	}
}