using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_up_Platform : MonoBehaviour
{

    private Rigidbody2D rb;

    public float Speed;

    private bool Position = true;

    public Transform top, buttom;

    private float topy, buttomy;
    public GameObject parent;
    private Transform playerDefTransform;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDefTransform = GameObject.FindGameObjectWithTag("link").transform.parent;
        transform.DetachChildren();
        topy = top.position.y;
        buttomy = buttom.position.y;
        Destroy(top.gameObject);
        Destroy(buttom.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (Position)
        {
            rb.velocity = new Vector2(rb.velocity.x, Speed);
            if (transform.position.y > topy)
            {
                Position = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -Speed);
            if (transform.position.y < buttomy)
            {
                Position = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("link"))
        {
            other.gameObject.transform.parent = parent.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("link"))
        {
            other.gameObject.transform.parent = playerDefTransform;
        }
    }
}
