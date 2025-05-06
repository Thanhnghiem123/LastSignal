
using System;
using UnityEngine;
using UnityEngine.Events;

public class InteractionEvent : MonoBehaviour
{
    public UnityEvent OnInteract;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        // Find the GameObject with the "Player" tag and get its PlayerHealth component
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found.");
        }
    }

    public void TryHealAndDestroy(float healAmount)
    {
        if (playerHealth != null && playerHealth.Heal(healAmount))
        {
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
