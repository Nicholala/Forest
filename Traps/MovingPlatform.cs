using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("属性")]
    public float speed;
    public float waitTime;
    public Transform[] movePos;

    private int i;
    private Transform playerDefTransform;
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        i = 1;
        playerDefTransform = GameObject.FindGameObjectWithTag("link").transform.parent;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, movePos[i].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, movePos[i].position) < 0.1f) 
        {
            if(waitTime <0.1f)
            {
                if (i == 0)
                    i = 1;
                else
                    i = 0;
                waitTime = 0.5f;
            }
            else
            {
                waitTime = Time.fixedDeltaTime;
            }      
        }   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("link"))
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
