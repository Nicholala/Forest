using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenStone : MonoBehaviour
{
    static public FallenStone S;
    [Header("属性")]
    public float gravity;
    public float UpSpeed;
    public Vector3 Des;
    public float AccelerateTime;
    public bool CanFall;
    public float TimeOnGround;
    float TimeToGround;
    

    [Header("触发判定")]
    public Vector2 TriggerPointOffset;
    public Vector2 TriggerJudgeSize;
    public LayerMask LinkLayerMask;

    bool isOnGround;
    float velocityX;
    public Rigidbody2D Rig;
    void Awake()

    {
        if (S == null)
        {
            S = this;
        }
        Rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CanFall = Trigger();
        if (CanFall)
        {
            Rig.gravityScale = gravity;
        }        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("link"))
        {
            Link.S.Alife = false;
            Rig.velocity = Vector2.zero;
            Rig.gravityScale = 0f;
        }
    }

    bool Trigger()
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + TriggerPointOffset, TriggerJudgeSize, 0, LinkLayerMask);
        return (Coll != null);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + TriggerPointOffset, TriggerJudgeSize);
    }
}
