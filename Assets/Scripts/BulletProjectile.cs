using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cinemachine;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
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

    private void OnTriggerEnter(Collider other)
    {
        //Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Paintable p = collision.collider.GetComponent<Paintable>();
        Vector3 pos = collision.contacts[0].point;
        if (p!=null)
        {
            PaintManager.instance.paint(p,pos,1.0f,0.5f,0.5f,Color.red);
        }
    }
}
