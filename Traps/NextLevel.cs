using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [Header("判定")]
    public LayerMask LinkLayerMask;
    public Vector2 PointOffset;
    public Vector2 JudgeSize;
    public bool TouchedByLink;

    Rigidbody2D Rig;

    // Update is called once per frame
    void Update()
    {
        TouchingLink();
        LevelUp();
    }

    public void LevelUp()
    {
        if (TouchedByLink)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
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
