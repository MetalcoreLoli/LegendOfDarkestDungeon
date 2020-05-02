﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{

    public float moveTime = 5f;
    public LayerMask blockingLayer;

    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rb2D;

    private float inverseMoveTime;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > 0.01)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.fixedDeltaTime);

            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }

    protected virtual bool Move(int xDir, int yDir, out RaycastHit2D hit) 
    {
        Vector2 start = transform.position;

        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
           StartCoroutine(SmoothMovement(end));
           //rb2D.MovePosition(end);

            return true;
        }
        return false;
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir) 
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
            return;

        T hitComp = hit.transform.GetComponent<T>();

        if (!canMove && hitComp != null)
            OnCantMove(hitComp);
    }

    protected abstract void OnCantMove<T>(T comp) where T : Component;
}
