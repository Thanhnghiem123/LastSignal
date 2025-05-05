using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float attackTimer;

    public override void Enter()
    {
        // Reset các giá trị khi vào trạng thái tấn công
        //attackTimer = 0;
        //moveTimer = 0;
        //losePlayerTimer = 0;
    }

    public override void Exit()
    {
        // Khi thoát khỏi trạng thái tấn công, có thể thêm một số logic nếu cần
    }

    public override void Perfrom()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            attackTimer += Time.deltaTime;

            enemy.setSpeedAttack();
            

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
            if (enemy.IsDead == false)
            {
                // Tính toán hướng đi của zombie về phía người chơi
                Vector3 directionToPlayer = (enemy.Player.transform.position - enemy.transform.position).normalized;
                //Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                //enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * 5f); // Quay mượt mà


                if (distanceToPlayer <= 2f)
                {
                    // Nếu trong phạm vi tấn công, dừng lại và tấn công
                    enemy.Agent.isStopped = true;
                    if (attackTimer > enemy.fireRate)
                    {
                        
                        enemy.Attack();
                        attackTimer = 0;
                    }
                }
                else
                {
                    // Nếu ngoài phạm vi tấn công,huy tan cong, di chuyển về phía người chơi
                    enemy.Animator.SetBool("IsAttacking", false);
                    enemy.Agent.isStopped = false;
                    enemy.Agent.SetDestination(enemy.Player.transform.position);
                    return;

                }
            }
            else
            {
                enemy.Agent.isStopped = true;
                //enemy.enabled = false; // Vô hiệu hóa toàn bộ script khi zombie chết
            }
            

            enemy.LastKnowPos = enemy.Player.transform.position; // Cập nhật vị trí cuối cùng của người chơi
        }
        else
        {
            // Nếu mất tầm nhìn của người chơi, đợi và chuyển sang trạng thái tìm kiếm
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                stateMachine.ChangState(new SearchState());
            }
        }
    }

    

}
