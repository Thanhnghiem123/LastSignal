using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private float health; // current health
    private float lerpTimer; // timer for lerping health bar
    private float durationTimer; // timer to check against the duration
    private bool isDead; // Trạng thái chết

    [Header("HealthBar")]
    public float maxHealth = 100;
    public float chipSpeed = 0.5f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;

    [Header("DamageOverlay")]
    public Image overlay; // GameObject DamageOverlay
    public float duration; // how long the image stays fully opaque
    public float fadeSpeed; // how quickly the image will fade

    [Header("Death Effect")]
    [SerializeField] private float deathTiltAngle = 90f; // Góc nghiêng camera khi chết (90 độ)
    [SerializeField] private float deathTiltDuration = 2f; // Thời gian nghiêng
    [SerializeField] private float fadeToBlackDuration = 2f; // Thời gian tối dần màn hình
    [SerializeField] public Image blackFadeImage; // Image màu đen để tối dần
    [SerializeField] private AudioSource deathSound; // Âm thanh chết (tùy chọn)

    private Camera mainCamera;
    private PlayerMovementScript movementScript;
    private MouseLookScript mouseLookScript;


    // get set health
    public float Health
    {
        get { return health; }
        set
        {
            health = value;
            health = Mathf.Clamp(health, 0, maxHealth);
            UpdateHealthUI();
        }
    }

    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0); // set alpha to 0
        if (blackFadeImage != null)
            blackFadeImage.color = new Color(0, 0, 0, 0); // Đảm bảo image đen trong suốt ban đầu
        isDead = false;

        // Tìm các component
        mainCamera = Camera.main;
        movementScript = GetComponent<PlayerMovementScript>();
        mouseLookScript = GetComponent<MouseLookScript>();

        if (mainCamera == null) Debug.LogError("Main Camera not found!");
        if (movementScript == null) Debug.LogError("PlayerMovementScript not found!");
        if (mouseLookScript == null) Debug.LogError("MouseLookScript not found!");
        if (blackFadeImage == null) Debug.LogError("Black Fade Image not assigned!");
    }

    void Update()
    {
        if (isDead) return; // Không update nếu đã chết

        UpdateHealthUI();

        // Kiểm tra nếu hết máu
        if (health <= 0)
        {
            StartCoroutine(DeathEffect());
        }

        //visualstudio // Hiệu ứng khi bị tổn thương
        if (overlay.color.a > 0)
        {
            if (health < 30)
                return; // Giữ overlay nếu máu thấp

            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction) // Khi bị tổn thương
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        else if (fillF < hFraction) // Khi hồi máu
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }

        if (healthText != null)
        {
            healthText.text = Mathf.Round(health) + " / " + maxHealth;
        }

        if (fillF != hFraction) lerpTimer = 0f;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // Không nhận sát thương nếu đã chết

        health -= damage;
        lerpTimer = 0f;
        health = Mathf.Clamp(health, 0, maxHealth);
        durationTimer = 0f;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.12f);
    }

    public bool Heal(float healAmount)
    {
        if (isDead) return false; // Không hồi máu nếu đã chết

        if (health != maxHealth)
        {
            health += healAmount;
            lerpTimer = 0f;
            health = Mathf.Clamp(health, 0, maxHealth);
            return true;
        }
        return false;
    }

    private IEnumerator DeathEffect()
    {
        isDead = true;

        // Khóa input
        if (movementScript != null) movementScript.enabled = false;
        if (mouseLookScript != null) mouseLookScript.enabled = false;

        // Phát âm thanh chết (nếu có)
        if (deathSound != null) deathSound.Play();

        // Tăng overlay (hiệu ứng máu/mờ)
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1f);

        // Lưu trạng thái ban đầu của camera
        Quaternion originalCamRot = mainCamera.transform.localRotation;

        // Giai đoạn ngã xuống (nghiêng 90 độ)
        float tiltTimer = 0f;
        Quaternion startRotation = mainCamera.transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y, deathTiltAngle);
        while (tiltTimer < deathTiltDuration)
        {
            tiltTimer += Time.deltaTime;
            float t = tiltTimer / deathTiltDuration;
            mainCamera.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        // Đảm bảo camera nằm đúng 90 độ
        mainCamera.transform.localRotation = endRotation;

        // Giai đoạn tối dần màn hình với blackFadeImage
        if (blackFadeImage != null)
        {
            float fadeTimer = 0f;
            Color startColor = new Color(0, 0, 0, 0); // Bắt đầu trong suốt
            Color endColor = new Color(0, 0, 0, 1f); // Kết thúc với đen hoàn toàn
            while (fadeTimer < fadeToBlackDuration)
            {
                fadeTimer += Time.deltaTime;
                float t = fadeTimer / fadeToBlackDuration;
                blackFadeImage.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }

            // Đảm bảo màn hình hoàn toàn đen
            blackFadeImage.color = endColor;
        }

        // Chuyển sang scene 0
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}