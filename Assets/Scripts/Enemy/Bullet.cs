using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        if(hitTransform.CompareTag("Player"))
        {
            hitTransform.GetComponent<PlayerHealth>().TakeDamage(10);
            Debug.Log("Hit Player");
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform hitTransform = other.transform;
        if (hitTransform.CompareTag("Player"))
        {
            hitTransform.GetComponent<PlayerHealth>().TakeDamage(10);
            Debug.Log("Hit Player");
        }
        Destroy (gameObject);
    }
}
