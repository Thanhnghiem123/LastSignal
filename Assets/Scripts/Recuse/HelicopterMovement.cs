using UnityEngine;

public class HelicopterMovement : MonoBehaviour
{
    public static HelicopterMovement Instance { get; private set; }

    [SerializeField] private float duration;
    [SerializeField] private float leaveDuration;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float pitchAngle;
    [SerializeField] private HelicopterTimerUI timerUI;
    [SerializeField] private IntroCutscene introCutscene;

    private Vector3 startPos;
    public Vector3 initialPos;
    private float currentDuration;
    private float elapsedTime;
    private bool isMoving;
    private bool hasStartedMoving;
    private BackgroudSoundManager soundManager;
    [SerializeField] private float speed = 10f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        soundManager = FindObjectOfType<BackgroudSoundManager>();
        if (soundManager == null) Debug.LogError("BackgroudSoundManager not found!");
        timerUI = FindObjectOfType<HelicopterTimerUI>();
        if (timerUI == null) Debug.LogError("HelicopterTimerUI not found!");
        introCutscene = FindObjectOfType<IntroCutscene>();
        if (introCutscene == null) Debug.LogError("IntroCutscene not found!");

        startPos = transform.position;
        initialPos = transform.position; // World position

        if (timerUI != null)
        {
            timerUI.OnTimerStart += OnTimerStarted; // Đăng ký một lần
        }
        if (introCutscene != null)
        {
            introCutscene.OnCutsceneFinished += OnCutsceneEnd;
        }
    }

    void OnDestroy()
    {
        if (timerUI != null) timerUI.OnTimerStart -= OnTimerStarted;
        if (introCutscene != null) introCutscene.OnCutsceneFinished -= OnCutsceneEnd;
    }

    void OnCutsceneEnd()
    {
        ActivateMovement();
    }

    void Update()
    {
        // Kiểm tra nếu timerUI đang chạy và còn 10 giây thì bắt đầu di chuyển
        if (timerUI != null && !hasStartedMoving && timerUI.RemainingTime <= 10f && timerUI.RemainingTime > 0)
        {
            StartMoving(speed); // Di chuyển trong thời gian speed
            hasStartedMoving = true; // Ngăn gọi lại
            Debug.Log($"StartMoving triggered at RemainingTime={timerUI.RemainingTime}");
        }

        if (!isMoving) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / currentDuration);
        float easedT = Mathf.SmoothStep(0f, 1f, t);
        transform.position = Vector3.Lerp(startPos, targetPosition.position, easedT);

        Vector3 direction = (targetPosition.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            float speedFactor = (t < 0.5f ? t : 1 - t) * 2;
            float pitch = -pitchAngle * speedFactor;
            targetRot = Quaternion.Euler(pitch, targetRot.eulerAngles.y, targetRot.eulerAngles.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        if (t >= 1f)
        {
            isMoving = false;
            transform.position = targetPosition.position;
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            Debug.Log($"Movement completed: pos={transform.position}, target={targetPosition.position}");
        }
    }

    void OnTimerStarted(float duration)
    {
        // Không gọi StartMoving ngay, chỉ reset trạng thái
        hasStartedMoving = false;
        Debug.Log($"Timer started with duration={duration}");
    }

    void StartMoving(float duration)
    {
        currentDuration = duration;
        elapsedTime = 0f;
        startPos = transform.position;
        isMoving = true;
        Debug.Log($"StartMoving: target={targetPosition.position}, duration={duration}, isMoving={isMoving}");
    }

    public void ActivateMovement()
    {
        timerUI.Activate(duration); // Kích hoạt timerUI
        Debug.Log($"ActivateMovement: Timer started with moveDuration={duration}");
    }

    public void LeaveTheCity()
    {
        currentDuration = leaveDuration;
        elapsedTime = 0f;
        startPos = transform.position;
        targetPosition.position = initialPos; // Mục tiêu là initialPos
        isMoving = true;
        soundManager?.StopAllSounds();
        soundManager?.PlaySoundByIndex(3);
        Debug.Log($"LeaveTheCity: target={targetPosition.position}, duration={leaveDuration}, isMoving={isMoving}");
    }
}