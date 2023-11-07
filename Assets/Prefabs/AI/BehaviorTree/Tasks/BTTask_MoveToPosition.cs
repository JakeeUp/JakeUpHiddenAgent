using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTTaks_MoveToPosition : BTNode
{
    [SerializeField]
    string targetKeyName = "nextPatrolPoint";

    [SerializeField]
    float acceptableDistance = 2.0f;

    NavMeshAgent agent;
    GameObject owner;
    Vector3 pos;
    Blackboard blackboard;

    protected override BTNodeResult Execute()
    {
        if (!GetBehaviorTree()) return BTNodeResult.Failure;

        blackboard = GetBehaviorTree().GetBlackBoard();
        if (blackboard == null) return BTNodeResult.Failure;

        if (!blackboard.GetBlackboarData("owner", out owner))
            return BTNodeResult.Failure;

        agent = owner.GetComponent<NavMeshAgent>();
        if (!agent) return BTNodeResult.Failure;

        if (!blackboard.GetBlackboarData(targetKeyName, out pos))
            return BTNodeResult.Failure;

        agent.stoppingDistance = acceptableDistance;
        agent.SetDestination(pos);
        agent.isStopped = false;

        return BTNodeResult.InProgress;
    }

    protected override BTNodeResult Update()
    {
        if (InAcceptableDistance())
        {
            return BTNodeResult.Success;
        }

        return BTNodeResult.InProgress;
    }

    private bool InAcceptableDistance()
    {
        return Vector3.Distance(owner.transform.position, pos) <= acceptableDistance;
    }

    public override void End()
    {
        if (agent) agent.isStopped = true;
        
            base.End();
        
       
        
    }
}
