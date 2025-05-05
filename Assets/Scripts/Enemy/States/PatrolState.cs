using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    public float waitTimer;
    public override void Enter()
    {
    }
    public override void Perfrom()
    {
        PatrolCycle();
        if(enemy.CanSeePlayer())
        {
            stateMachine.ChangState(new AttackState());
        }
    }
    public override void Exit()
    {
    }

    

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f) // Khoang cach hien tai den dich
        {
            waitTimer += Time.deltaTime;
            if(waitTimer > 3) // Sau 3s tiep tuc di den point tiep theo
            {
                if (waypointIndex < enemy.path.waypoints.Count) // Di chuyen den tat ca cac points
                    waypointIndex++;
                else
                    waypointIndex = 0; // Khi di chuyen den dich reset lai
                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position); // Phuong thuc SetDestination - di chuyen doi tuong den vi tri mong muon
                waitTimer = 0; // reset
            }
        }
    }
}
