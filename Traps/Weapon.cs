using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("参数")]
    public GameObject projectilePrefab;
    public Vector2 dir;
    public float projectileSpeed;
    public float timeBetweenShot;
    public float lastshot;
    
    // Start is called before the first frame update
    void Start()
    {
        lastshot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastshot > timeBetweenShot)
        {
            Fire();
            lastshot = Time.time;
        }
    }
    void Fire()
    {
        GameObject proGO = Instantiate<GameObject>(projectilePrefab);
        proGO.transform.position = transform.position;
        Rigidbody2D RigidB = proGO.GetComponent<Rigidbody2D>();
        RigidB.velocity = dir * projectileSpeed;
    }
}
