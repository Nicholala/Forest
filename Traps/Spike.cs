using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [Header("地刺判定")]
    public LayerMask LinkLayerMask;

    Rigidbody2D Rig;
    // Start is called before the first frame update
    void Awake()
    {
        Rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("link"))
        {
            Link.S.Alife=false;
        }
    }
}
