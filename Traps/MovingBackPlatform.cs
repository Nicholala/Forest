using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackPlatform : MonoBehaviour
{
    [Header("属性")]
    public bool Active;
    public bool Started;
    public bool isMoving;
    public bool isForward;
    public float speedGO;
    public float speedBack;
    public float waitTime;
    public float AccelerateTime;
    public Transform[] movePos;

    private float beginTime;
    private Transform playerDefTransform;
    public GameObject parent;
    float velocityX;
    Rigidbody2D Rig;

    // Start is called before the first frame update
    void Start()
    {
        Active = false;
        Rig = GetComponent<Rigidbody2D>();
        playerDefTransform = GameObject.FindGameObjectWithTag("link").transform.parent;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Started)
        {
            if (Time.time - beginTime > waitTime)
            {
                Active = true;
            }
        }     
        if (Active)
        {
            isMoving = true;
            if (isForward)
            {
                float tempv = Mathf.SmoothDamp(Rig.velocity.x, speedGO * Time.fixedDeltaTime * 60, ref velocityX, AccelerateTime);
                transform.position = Vector2.MoveTowards(transform.position, movePos[1].position, tempv);
                if (Vector2.Distance(transform.position, movePos[1].position) < 0.1f)
                {
                    isForward = false;
                    Active = false;
                    beginTime = Time.time;
                }
            }
            else
            {
                float tempv = Mathf.SmoothDamp(Rig.velocity.x, speedBack * Time.fixedDeltaTime * 60, ref velocityX, AccelerateTime);
                transform.position = Vector2.MoveTowards(transform.position, movePos[0].position, tempv);
                if (Vector2.Distance(transform.position, movePos[0].position) < 0.1f)
                {
                    isForward = true;
                    isMoving = false;
                    Active = false;
                    Started = false;
                }
            }
        }       
    } 
            

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("link"))
        {
            if (!isMoving) Started = true;
            beginTime = Time.time;
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
