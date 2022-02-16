using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cinemachine;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [Space]
    public Color paintColor;
    public float radius;
    public float hardness;
    public float strength;

    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 10.0f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Paintable p = collision.collider.GetComponent<Paintable>();
        Vector3 pos = collision.contacts[0].point;
        if (p!=null)
        {
            //Why I can not use public vector for paintColor here
            Debug.Log(1);
            PaintManager.instance.paint(p,pos, radius, hardness, strength, Color.red);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Destroy(gameObject);
    }
}
