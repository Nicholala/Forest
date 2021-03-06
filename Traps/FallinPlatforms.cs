using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallinPlatforms : MonoBehaviour
{

    public float fallingtime = 0;
    private Rigidbody2D _rigidbody2D;


    public float Fallingspeed;

    public Transform Buttom;

    private float Buttomy;

    //Animator animator;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        Buttomy = Buttom.position.y;
        Destroy(Buttom.gameObject);
        //animator = GetComponent<Animator>();

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "link")
        {
             //animator.Play("Falling_on");
            Falling();

        }
    }

    private void Falling()
    {
  
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -Fallingspeed);
        if (transform.position.y < Buttomy)
        {
            _rigidbody2D.velocity = new Vector2(0, 0);
            //_rigidbody2D.isKinematic = false;
            //_rigidbody2D.gravityScale = 2f;
        }

    }

}
