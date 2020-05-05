using Assets.Scripts.Dices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{


    public int Hp = DiceManager.RollUndSumFromString("2d4");

    public int Damage = DiceManager.SixEdges.Roll();

	private Animator animator;                          
	private Transform target;                           
	private bool skipMove;


	private void FixedUpdate()
	{
		MoveEnemy();
		if (Hp <= 0)
		{
			GameManager._instance.Enemies.Remove(this);
			Destroy(gameObject);
		}	
	}

	protected override void Start()
	{
		GameManager._instance.Enemies.Add(this);

		animator = GetComponent<Animator>();

		target = GameObject.FindGameObjectWithTag("Player").transform;

		base.Start();
	}

	//protected override bool Move(int xDir, int yDir, out RaycastHit2D hit)
	//{
	//	Vector2 start = transform.position;

	//	Vector2 end = start + new Vector2(xDir, yDir);

	//	boxCollider.enabled = false;

	//	hit = Physics2D.Linecast(start, end, blockingLayer);

	//	boxCollider.enabled = true;

	//	if (hit.transform == null && !isMoving)
	//	{
	//		//StartCoroutine(SmoothMovement(end));
	//		rb2D.MovePosition(end);
	//		return true;
	//	}

	//	return false;
	//}

	//protected override void AttemptMove<T>(int xDir, int yDir)
	//{
	//	if (skipMove)
	//	{
	//		skipMove = false;
	//		return;

	//	}

	//	RaycastHit2D hit;

	//	bool canMove = Move(xDir, yDir, out hit);

	//	if (hit.transform == null)
	//		return;

	//	T hitComponent = hit.transform.GetComponent<T>();

	//	if (!canMove && hitComponent != null)
	//		OnCantMove(hitComponent);
	//	skipMove = true;
	//}

	public void MoveEnemy()
	{
		int xDir = 0;
		int yDir = 0;

		//if (Mathf.Abs(target.position.x - (transform.position.x)) < float.Epsilon)
		//	yDir = 1;
		//else
		//	xDir = 0;
		//var dir = (target.position - transform.position).normalized;
		//AttemptMove<Player>(Mathf.RoundToInt(dir.x), Mathf.RoundToInt(dir.y));

		if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
			yDir = target.position.y > transform.position.y ? 1 : -1;
		else
			xDir = target.position.x > transform.position.x ? 1 : -1;
		//rb2D.MovePosition()
		Debug.Log($"Moving to {xDir}{yDir}");
		AttemptMove<Player>(xDir, yDir);
	}

	protected override void AttemptMove<T>(int xDir, int yDir)
	{
		if (skipMove)
		{
			skipMove = false;
			return;

		}

		//Call the AttemptMove function from MovingObject.
		base.AttemptMove<T>(xDir, yDir);

		//Now that Enemy has moved, set skipMove to true to skip next move.
		skipMove = true;
	}

	protected override void OnCantMove<T>(T component)
	{
		Player hitPlayer = component as Player;

		hitPlayer.LoseHp(Damage);

	}
}
