using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("判定")]
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
        GameObject GO = other.gameObject;
        switch (GO.tag)
        {
            case "link":
                Link.S.Alife = false;
                Destroy(gameObject);
                break;
            case "EndPlace":
                Destroy(gameObject);
                break;
            case "ground":
                Destroy(gameObject);
                break;
        }
       /* if (other.gameObject.CompareTag("link"))
        {
            Link.S.Alife = false;
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("EndPlace")|| other.gameObject.CompareTag("ground"))
        {
            Destroy(gameObject);
        }*/
    }
}
