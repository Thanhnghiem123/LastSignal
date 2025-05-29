using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GunScript;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine; // Trạng thái của enemy
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 lastKnowPos;
    private Animator animator;
    private bool isDead = false; // Cờ kiểm tra zombie đã chết
    private bool isHit = false; // Cờ kiểm tra zombie đang bị đánh
    private Coroutine speedUpCoroutine;

    public bool IsDead { get => isDead; }
    public Animator Animator { get => animator; }
    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }
    public Vector3 LastKnowPos { get => lastKnowPos; set => lastKnowPos = value; }

    [Header("Movement Values")]
    public float speed;
    public Path path;
    public GameObject debugsphere;

    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;
    public float hearingRange = 180f;

    [Header("Weapon Values")]
    public Transform gunBarrel;
    [Range(0.1f, 10f)]
    public float fireRate;

    [Header("Animator Parameters")]
    public float stoppingDistance = 2f; // Khoảng cách dừng để tấn công

    [Header("Audio")]
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource walkSound;

    [Header("State")]
    [SerializeField] private string currentState;

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (isDead || isHit) return;

        if (SoundManager.soundMade) // Nếu có âm thanh
        {
            float distanceToSound = Vector3.Distance(transform.position, SoundManager.lastSoundPosition);
            if (distanceToSound <= hearingRange) // Nếu âm thanh trong phạm vi nghe
            {
                Animator.SetBool("AttackState", true);
                setSpeedAttack();
                //agent.SetDestination(SoundManager.lastSoundPosition);
                Debug.Log("Zombie đang di chuyển đến vị trí âm thanh: " + SoundManager.lastSoundPosition);
                if (!agent.hasPath)
                {
                    //Debug.Log("Không tìm thấy đường đi đến đích!");
                }
                else
                {
                    //Debug.Log("Đường đi đã được tìm thấy!");
                }
                //Debug.Log("distanceToSound " + distanceToSound);
                //Debug.Log("hearingRange " + hearingRange);

            }
            StartCoroutine(ResetSoundAfterFrame()); // Reset sau một frame
            
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            CanSeePlayer();
            currentState = stateMachine.activeState.ToString();
            UpdateAIState();
        }
    }
    private IEnumerator ResetSoundAfterFrame()
    {
        yield return new WaitForEndOfFrame();
        SoundManager.soundMade = false;
    }

    public bool CanSeePlayer()
    {
        if (isDead || isHit) return false; // Không nhìn thấy player nếu đã chết hoặc đang bị đánh

        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Vector3 rayOrigin = transform.position + Vector3.up * eyeHeight;
                    Ray ray = new Ray(rayOrigin, targetDirection);
                    RaycastHit raycastHit;
                    if (Physics.Raycast(ray, out raycastHit, sightDistance))
                    {
                        if (raycastHit.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            Animator.SetBool("AttackState", true);
                            //agent.SetDestination(player.transform.position);
                            return true;
                        }
                    }
                }
            }
        }
        //Animator.SetBool("AttackState", false);
        return false;
    }

    private void UpdateAIState()
    {
        setSpeed();
    }

    public void setSpeed()
    {
        if (isHit) return; // Không cập nhật tốc độ nếu đang bị đánh
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    public void setSpeedAttack()
    {
        if (isHit) return; // Không thay đổi tốc độ nếu đang bị đánh
        fieldOfView = 360f;
        agent.speed = 5f;
        attackSound.Play();
        walkSound.Stop();
    }

    public void setSpeedPatrol()
    {
        if (isHit) return; // Không thay đổi tốc độ nếu đang bị đánh
        agent.speed = 0.7f;
    }

    public void HealthControl(float health)
    {
        if (isHit) return; // Không cập nhật health nếu đang bị đánh
        animator.SetFloat("Health", health);
    }

    public void Attack()
    {
        if (isDead || isHit) return; // Không tấn công nếu đã chết hoặc đang bị đánh
        Animator.SetBool("IsAttacking", true);
        StartCoroutine(DealDamageWithDelay()); // Gọi coroutine để gây sát thương sau 0.4s
    }

    private IEnumerator DealDamageWithDelay()
    {
        yield return new WaitForSeconds(0.4f); // Chờ 0.4 giây
        if (isDead || isHit) yield break; // Kiểm tra lại trạng thái trước khi gây sát thương
        if (Player != null) // Đảm bảo Player không bị null
        {
            Player.GetComponent<PlayerHealth>().TakeDamage(5); // Gây sát thương
        }
    }

    public void GetHit()
    {
        if (isDead) return;
        StartCoroutine(HandleGetHit());
    }

    private IEnumerator HandleGetHit()
    {
        isHit = true; // Đánh dấu zombie đang bị đánh
        agent.isStopped = true; // Dừng NavMeshAgent
        agent.speed = Mathf.Clamp(agent.speed - 100f, 0f, 5f);
        Animator.SetTrigger("GetHit");
        yield return new WaitForSeconds(1f);
        agent.isStopped = false; // Tiếp tục di chuyển
        agent.speed = 5f; // Reset tốc độ
        isHit = false; // Reset trạng thái
    }

    public void Roar()
    {
        if (isDead || isHit) return; // Không gầm thét nếu đã chết hoặc đang bị đánh
        Animator.SetTrigger("Roar");
    }

    public void Die()
    {
        if (isDead) return;
        animator.SetTrigger("IsDead");
        agent.isStopped = true;
        isDead = true;
        attackSound.Stop();
        walkSound.Stop();
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            capsuleCollider.enabled = false;
        }
        if (agent != null)
        {
            agent.enabled = false;
        }
    }
}