using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private Enemy enemy;

    protected float health;
    protected float lerpTimer;
    protected float durationTimer; // timer to check against the duration


    [Header("HealtherBar")]
    public float maxHealth = 100;
    public float chipSpeed = 0.5f;
    public Image frontHealthBar;
    public Image backHealthBar;


    public WaveManager waveManager; // Gắn WaveManager vào

    void OnDisable() // Gọi khi zombie bị tắt
    {
        if (waveManager != null)
        {
            waveManager.OnZombieDeactivated(); // Báo cho WaveManager
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateHealthUI();
        enemy.HealthControl(health);
        if (health <= 0)
        {
            enemy.Die();
            return;
        }
    }

    public void UpdateHealthUI()
    {
        if(frontHealthBar!=null && backHealthBar!=null)
        {
            float fillF = frontHealthBar.fillAmount;
            float fillB = backHealthBar.fillAmount;
            float hFraction = health / maxHealth;

            if (fillB > hFraction) // when player is damaged
            {
                frontHealthBar.fillAmount = hFraction;
                backHealthBar.color = Color.red;
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / chipSpeed;
                backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
            }
            else if (fillF < hFraction) // when player is restored health
            {
                backHealthBar.fillAmount = hFraction;
                backHealthBar.color = Color.green;
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / chipSpeed;
                frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
            }

            // update text display hp

            // Reset timer
            if (fillF != hFraction) lerpTimer = 0f;
        }
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        health = Mathf.Clamp(health, 0, maxHealth);
        durationTimer = 0f;
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

}
