using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private float health;
    private PlayerHealth playerHealth;
    private GunScript gunScript;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        GameManager.Instance.ApplyGameData(gameObject);
    }

    
    void Update()
    {
       
    }

    public void SetHealth(float newHealth)
    {
        if (playerHealth != null)
        {
            playerHealth.Health = newHealth; // Thiết lập giá trị mới cho Health
            Debug.Log("Health set to: " + playerHealth.Health);
        }
        else
        {
            Debug.LogError("PlayerHealth component is not assigned!");
        }
    }

    //public void SetAmmo(float newBulletsIHave, float newBulletsInTheGun, float newAmountOfBulletsPerLoad)
    //{
    //    if (gunScript == null)
    //    {
    //        Debug.LogError("gunScript is null in SetAmmo!");
    //        return;
    //    }
    //    gunScript.bulletsIHave = newBulletsIHave;
    //    gunScript.bulletsInTheGun = newBulletsInTheGun;
    //    gunScript.amountOfBulletsPerLoad = newAmountOfBulletsPerLoad;
    //    Debug.Log($"Ammo set to: {gunScript.bulletsIHave}, {gunScript.bulletsInTheGun}, {gunScript.amountOfBulletsPerLoad}");
    //}

    public void SaveGame()
    {
            GameManager.Instance.SaveGameState(playerHealth.Health, transform.localPosition);
            Debug.Log("Game state saved! Position: " + transform.localPosition);
        
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor.
        Cursor.visible = true; // Make the cursor visible.
    }
}