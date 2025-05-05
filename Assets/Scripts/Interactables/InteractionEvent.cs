
using System;
using UnityEngine;
using UnityEngine.Events;

public class InteractionEvent : MonoBehaviour
{
    public UnityEvent OnInteract;
    public PlayerHealth playerHealth;



    public void TryHealAndDestroy(float healAmount)
    {
        if (playerHealth.Heal(healAmount))
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
