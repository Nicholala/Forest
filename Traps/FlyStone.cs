using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyStone : MonoBehaviour
{
    [Header("属性")]
    public int Mode;
    public float Speed;

    [Header("水平移动")]
    public Vector3 ChangePlace1;//左换位点
    public Vector3 ChangePlace2;//右换位点
    public Vector3 BornPlace;

    [Header("圆周运动")]
    public float radius;
    float x;
    float y;
    float w;

    [Header("判定")]
    public LayerMask LinkLayerMask;
    public bool TouchedByLink;
    public Vector2 FlyStonePointOffset;
    public Vector2 FlyStoneJudgeSize;

    public Transform playerDefTransform;
    public GameObject parent;

    void Awake()
    {
        BornPlace = transform.position;
        playerDefTransform = GameObject.FindGameObjectWithTag("link").transform.parent;
    }

    void Update()
    {
        Move();
        TouchingLink();

    }

    public void Move()
    {
        //水平移动
        if (Mode == 0)
        {
            transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
            if (transform.position.x <= ChangePlace1.x || transform.position.x >= ChangePlace2.x)
            {
                Speed *= -1;
            }
        }
        //圆周运动
        if (Mode == 1)
        {
            w += Speed * Time.deltaTime;
            x = Mathf.Cos(w) * radius;
            y = Mathf.Sin(w) * radius;
            transform.position = BornPlace + new Vector3(x, y, transform.position.z);
        }
    }

    public void TouchingLink()
    {
        Collider2D Coll = Physics2D.OverlapBox((Vector2)transform.position + FlyStonePointOffset, FlyStoneJudgeSize, 0, LinkLayerMask);
        if (Coll != null)
        {
            TouchedByLink = true;
            Link.S.Alife = false;
        }
        return;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position + FlyStonePointOffset, FlyStoneJudgeSize);
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
