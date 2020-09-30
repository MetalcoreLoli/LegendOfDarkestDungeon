using UnityEngine;

public class Wall : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject wasHitUp;
    [SerializeField] private GameObject wasHitDown;
    [SerializeField] private GameObject wasHitLeft;
    [SerializeField] private GameObject wasHitRight;

    private BoxCollider2D boxCollider2;

    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2 = GetComponent<BoxCollider2D>();
    }

    public bool HitUpLeftWithTag(string tag)
    {
        var gb = HitUpOrDefault();
        var gb1 = HitLeftOrDefault();
        var free = HitDownOrDefault();
        var free1 = HitRightOrDefault();

        if (gb != null && gb1 != null && (free == null || free1 == null))
            return gb.CompareTag(tag) && gb1.CompareTag(tag);
        else
            return false;
    }

    public bool HitUpRightWithTag(string tag)
    {
        var gb = HitUpOrDefault();
        var gb1 = HitRightOrDefault();
        var free = HitDownOrDefault();
        var free1 = HitLeftOrDefault();

        if (gb != null && gb1 != null && (free == null || free1 == null))
            return gb.CompareTag(tag) && gb1.CompareTag(tag);
        else
            return false;
    }

    public bool HitDownLeftWithTag(string tag)
    {
        var gb = HitDownOrDefault();
        var gb1 = HitLeftOrDefault();
        var free = HitUpOrDefault();
        var free1 = HitRightOrDefault();

        if (gb != null && gb1 != null && (free == null || free1 == null))
            return gb.CompareTag(tag) && gb1.CompareTag(tag);
        else
            return false;
    }

    public bool HitDownRightWithTag(string tag)
    {
        var gb = HitDownOrDefault();
        var gb1 = HitRightOrDefault();
        var free = HitUpOrDefault();
        var free1 = HitLeftOrDefault();

        if (gb != null && gb1 != null && (free == null || free1 == null))
            return gb.CompareTag(tag) && gb1.CompareTag(tag);
        else
            return false;
    }

    public bool HitLeftRightWithTag(string tag)
    {
        var gb = HitLeftOrDefault();
        var gb1 = HitRightOrDefault();
        //var free = HitUpOrDefault();
        //var free1 = HitDownOrDefault();

        if (gb != null && gb1 != null)
            return gb.CompareTag(tag) && gb1.CompareTag(tag);
        else
            return false;
    }

    public bool HitUpDownWithTag(string tag)
    {
        var gb = HitUpOrDefault();
        var gb1 = HitDownOrDefault();
        //var free = HitUpOrDefault();
        //var free1 = HitDownOrDefault();

        if (gb != null && gb1 != null)
            return gb.CompareTag(tag) && gb1.CompareTag(tag);
        else
            return false;
    }

    public bool HitDownLeftRightWithTag(string tag)
    {
        var gb = HitLeftOrDefault();
        var gb1 = HitRightOrDefault();
        var gb2 = HitDownOrDefault();
        //var free = HitUpOrDefault();
        //var free1 = HitDownOrDefault();

        if (gb != null && gb1 != null && gb2 != null)
            return gb.CompareTag(tag) && gb1.CompareTag(tag) && gb2.CompareTag(tag);
        else
            return false;
    }

    public bool HitLeftWithTag(string tag)
    {
        var gb = HitLeftOrDefault();

        if (gb != null)
            return gb.CompareTag(tag);
        else
            return false;
    }

    public bool HitRightWithTag(string tag)
    {
        var gb = HitRightOrDefault();

        if (gb != null)
            return gb.CompareTag(tag);
        else
            return false;
    }

    public bool HitUpWithTag(string tag)
    {
        var gb = HitUpOrDefault();

        if (gb != null)
            return gb.CompareTag(tag);
        else
            return false;
    }

    public bool HitDownWithTag(string tag)
    {
        var gb = HitDownOrDefault();

        if (gb != null)
            return gb.CompareTag(tag);
        else
            return false;
    }

    public bool IsAngelHere() => HitDownRightWithTag("Wall") || HitDownLeftWithTag("Wall") || HitUpLeftWithTag("Wall") || HitUpRightWithTag("Wall");

    public GameObject HitUpOrDefault()
    {
        boxCollider2.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.up, 1);
        boxCollider2.enabled = true;

        if (hit.transform != null)
            return hit.transform.gameObject;
        else
            return default;
    }

    public GameObject HitDownOrDefault()
    {
        boxCollider2.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 1);
        boxCollider2.enabled = true;

        if (hit.transform != null)
            return hit.transform.gameObject;
        else
            return default;
    }

    public GameObject HitLeftOrDefault()
    {
        boxCollider2.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.left, 1);
        boxCollider2.enabled = true;

        if (hit.transform != null)
            return hit.transform.gameObject;
        else
            return default;
    }

    public GameObject HitRightOrDefault()
    {
        boxCollider2.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right, 1);
        boxCollider2.enabled = true;

        if (hit.transform != null)
            return hit.transform.gameObject;
        else
            return default;
    }
}