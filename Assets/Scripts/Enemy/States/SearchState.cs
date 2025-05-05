using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;
    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnowPos);
    }

    public override void Exit()
    {
    }

    public override void Perfrom()
    {
        if(enemy.CanSeePlayer())
        {
            stateMachine.ChangState(new AttackState());
        }

        if(enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;
            if (moveTimer > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 2));
                moveTimer = 0;
            }
            if (searchTimer > 8)
            {
                //stateMachine.ChangState(new PatrolState());
                enemy.Agent.SetDestination(enemy.Player.transform.position);
            }
        }
    }
}
