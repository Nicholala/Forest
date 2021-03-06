using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelicatePlatform : MonoBehaviour
{

    [Header("属性")]
    public float LifeTime;
    public float BirthTime;

    [Header("判定")]
    public LayerMask LinkLayerMask;
    public Vector2 PointOffset;
    public Vector2 JudgeSize;
    public bool TouchedByLink;
    public bool CanBeDestroyed = false ;
    public bool BeginDestroy = false;

    Rigidbody2D Rig;
    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("BeginDestroy", BeginDestroy);
        Broken();
        TouchingLink();
    }

    public void Broken()
    {
        if (TouchedByLink&&CanBeDestroyed ==false)
        {
            BirthTime = Time.time;
            CanBeDestroyed = true;
        }
        if(Time.time - BirthTime > LifeTime&&CanBeDestroyed )
        {
            BeginDestroy = true;
        }
    }
    public void Des()
    {
        Destroy(gameObject);
    }
    public void TouchingLink()
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + PointOffset, JudgeSize, 0, LinkLayerMask);
        if (Coll != null)
        {
            TouchedByLink = true;
        }
        return;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position + PointOffset, JudgeSize);
    }
}