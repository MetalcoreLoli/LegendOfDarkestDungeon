using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private LayerMask blockingLayer;
    [SerializeField] private GameObject wasLeftHit;
    [SerializeField] private GameObject wasRightHit;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsClosed", false);
        LinecastLeft();
        LinecastRight();
    }

    // Update is called once per frame
    private void Update()
    {
        // GetComponent<BoxCollider2D>().enabled = true;
        // Debug.DrawRay(transform.position, Vector2.right, Color.green);

        if (wasLeftHit != null)
            Debug.DrawRay(transform.position, (wasLeftHit.transform.position - transform.position).normalized, Color.green);
        if (wasRightHit != null)
            Debug.DrawRay(transform.position, (wasRightHit.transform.position - transform.position).normalized, Color.green);
    }

    private void LinecastLeft()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        RaycastHit2D hit_left = Physics2D.Raycast(transform.position, new Vector3(-1, 0), 1);
        GetComponent<BoxCollider2D>().enabled = true;

        if (hit_left.transform != null)
        {
            if (hit_left.transform.gameObject.CompareTag("Wall"))
                Debug.DrawRay(transform.position, new Vector3(-1, 0), Color.red);
            wasLeftHit = hit_left.transform.gameObject;
        }
    }

    private void LinecastRight()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        RaycastHit2D hit_right = Physics2D.Raycast(transform.position, new Vector3(1, 0), 1);

        GetComponent<BoxCollider2D>().enabled = true;

        if (hit_right.transform != null)
        {
            if (hit_right.transform.gameObject.CompareTag("Wall"))
                Debug.DrawRay(transform.position, Vector2.right, Color.red);
            wasRightHit = hit_right.transform.gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //gameObject.layer = AfterEnter;

        if (collision.CompareTag("Player"))
        {
            animator.SetBool("IsClosed", true);
        }

        //GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("IsClosed", false);
        }
    }
}