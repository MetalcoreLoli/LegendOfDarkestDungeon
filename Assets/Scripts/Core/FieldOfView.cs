using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float ViewRadius;

    [Range(0, 360)]
    public float ViewAngle;

    public LayerMask targets;
    public LayerMask blockingMask;

    public List<Transform> visibleTargets;

    private void Start()
    {
        StartCoroutine(FinTargetsDelay(0.1f));
    }

    private IEnumerator FinTargetsDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindTargets();
        }
    }

    private void FindTargets()
    {
        visibleTargets.Clear();
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), ViewRadius, targets);
        foreach (var item in collider2Ds)
        {
            Transform t_transform = item.transform;
            Vector3 dir = (t_transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dir) < ViewAngle)
            {
                float dis = Vector3.Distance(transform.position, t_transform.position);
                if (!Physics2D.Raycast(transform.position, dir, dis, blockingMask))
                {
                    visibleTargets.Add(t_transform);
                }
            }
        }
    }

    public Vector2 DirFromAngel(float angle, bool isAngelGlobal = false)
    {
        if (!isAngelGlobal)
            angle -= transform.eulerAngles.z;
        return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
    }
}