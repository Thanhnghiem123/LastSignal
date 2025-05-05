using UnityEngine;
using System.Collections;

public class SwordScript : MonoBehaviour
{
    


    [Header("Sword Attack Properties")]
    public bool isAttacking = false; // Kiểm tra trạng thái tấn công
    public Animator handsAnimator;  // Animator của tay người chơi
    public string attackAnimationName = "meeleAttack"; // Tên của animation tấn công kiếm

    [Header("Sensitivity Settings")]
    public float mouseSensitivityNotAiming = 10f;
    public float mouseSensitivityAiming = 5f;
    public float rotationLagTime = 0.1f; // Thời gian chậm khi xoay kiếm

    [Header("Player Movement Properties")]
    public int walkingSpeed = 3;
    public int runningSpeed = 5;

    // Thêm biến này để điều chỉnh vị trí của kiếm trong Inspector
    public Vector3 swordOffset = new Vector3(0.04f, -1.3f, -0.06f); // Giá trị này có thể điều chỉnh trong Unity

    [Header("Audio for Attacking")]
    public AudioSource swordHitEnemy, swordHitLevel, swordHit;


    private Transform player;
    private Camera cameraComponent;
    private PlayerMovementScript pmS;
    private MouseLookScript mls;


    private float currentSpeed;



    [HideInInspector]
    public Transform mainCamera;
    private Vector3 velV;


    private Animator animator;
    private int attackStage = 0;

    

    void Awake()
    {
        mls = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLookScript>();
        player = mls.transform;
        cameraComponent = mls.myCamera.GetComponent<Camera>();
        pmS = player.GetComponent<PlayerMovementScript>();

        // Gán mainCamera từ cameraComponent
        mainCamera = cameraComponent.transform;  // Gán mainCamera ở đây

        animator = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        HandleSwordPosition();
        HandleSwordRotation();
        HandleAttackInput();
        HandleMovementSpeed();
        
    }
    private void FixedUpdate()
    {
        Attack();
    }








    // Xử lý vị trí kiếm khi người chơi di chuyển và nhắm
    void HandleSwordPosition()
    {
        // Tính toán vị trí mục tiêu dựa trên vị trí camera và offset
        Vector3 targetPosition = mainCamera.transform.position +
                                 (mainCamera.transform.right * swordOffset.x) + // Điều chỉnh vị trí ngang (trái/phải)
                                 (mainCamera.transform.up * swordOffset.y) +    // Điều chỉnh vị trí dọc (trên/dưới)
                                 (mainCamera.transform.forward * swordOffset.z); // Điều chỉnh vị trí trước/sau

        // Cập nhật vị trí kiếm với hiệu ứng chuyển động mượt mà
        transform.position = Vector3.SmoothDamp(targetPosition, targetPosition, ref velV, 0.025f);
    }


    // Xử lý xoay kiếm theo hướng camera (giống như súng)
    void HandleSwordRotation()
    {
        float rotationY = mls.currentYRotation;
        float rotationX = mls.currentCameraXRotation;

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0.5f);
    }

    // Kiểm tra tấn công kiếm
    void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isAttacking) // Sử dụng phím Q để tấn công
        {
            StartCoroutine(SwordAttack());
        }
    }

    // Animation tấn công kiếm
    IEnumerator SwordAttack()
    {
        isAttacking = true;
        animator.SetBool(attackAnimationName, true);

        // Giới hạn thời gian tấn công, sau đó quay lại trạng thái bình thường
        yield return new WaitForSeconds(0.8f); // Giả sử animation tấn công kéo dài 0.5s
        animator.SetBool(attackAnimationName, false);
        isAttacking = false;
    }

    // Xử lý tốc độ khi cầm kiếm
    void HandleMovementSpeed()
    {
        // Kiểm tra xem người chơi có đang chạy hoặc di chuyển không
        if (Input.GetAxis("Vertical") > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) // Khi nhấn Shift
            {
                if (pmS.maxSpeed == walkingSpeed)
                {
                    pmS.maxSpeed = runningSpeed;  // Tăng tốc độ khi chạy
                    animator.SetInteger("maxSpeed", runningSpeed); // Set giá trị MaxSpeed vào Animator
                }
                else
                {
                    pmS.maxSpeed = walkingSpeed; // Trở lại tốc độ đi bộ
                    animator.SetInteger("maxSpeed", walkingSpeed); // Set giá trị WalkingSpeed vào Animator
                }
            }
        }
        else
        {
            pmS.maxSpeed = walkingSpeed; // Nếu không di chuyển, đặt lại tốc độ
            animator.SetInteger("maxSpeed", walkingSpeed); // Set lại giá trị WalkingSpeed vào Animator
        }

        // Set giá trị WalkingSpeed vào Animator (nếu cần)
        animator.SetFloat("walkSpeed", walkingSpeed);
    }


    // Hàm cho phép chuyển giữa các vũ khí
    public void EquipSword(bool equip)
    {
        if (equip)
        {
            this.gameObject.SetActive(true); // Hiển thị kiếm
            pmS.maxSpeed = walkingSpeed; // Đảm bảo tốc độ di chuyển khi cầm kiếm là đi bộ
        }
        else
        {
            this.gameObject.SetActive(false); // Ẩn kiếm
        }
    }

    public void Attack()
    {

        if (Input.GetButtonDown("Fire1")) // "Fire1" là chuột trái hoặc nút attack
        {
            swordHit.Play(); // Phát âm thanh tấn công
            animator.SetTrigger("Attack");



            attackStage = Random.Range(1, 3); // Random giữa 1 và 2
            Debug.Log("Attack Stageaaaaaaaaaaaaaaaaaaaaaaaaaaaa: " + attackStage);
            animator.SetInteger("AttackStage", attackStage);

        }
        else if (Input.GetButtonDown("Fire2")) // "Fire2" là chuột phải hoặc nút block
        {
            swordHit.Play(); // Phát âm thanh tấn công
            animator.SetTrigger("AttackSpecial");
        }

    }
}

