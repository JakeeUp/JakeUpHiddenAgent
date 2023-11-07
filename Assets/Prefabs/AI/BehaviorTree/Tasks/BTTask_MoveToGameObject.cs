using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTTask_MoveToGameObject : BTNode
{
    [SerializeField]
    string targetKeyName = "target";

    [SerializeField]
    float acceptableDistance = 2.0f;

    NavMeshAgent agent;
    GameObject owner;
    GameObject target;
    Blackboard blackboard;

    protected override BTNodeResult Execute()
    {
        if (!GetBehaviorTree()) return BTNodeResult.Failure;

        blackboard = GetBehaviorTree().GetBlackBoard();
        if(blackboard == null) return BTNodeResult.Failure;

        if (!blackboard.GetBlackboarData("owner", out owner))
            return BTNodeResult.Failure;

        agent = owner.GetComponent<NavMeshAgent>();
        if(!agent) return BTNodeResult.Failure;

        if (!blackboard.GetBlackboarData(targetKeyName, out target))
            return BTNodeResult.Failure;

        //keep our target value up to date
        blackboard.onBlackboardValueChanged -= BlackboardValuedChanged;
        blackboard.onBlackboardValueChanged += BlackboardValuedChanged;

        agent.stoppingDistance = acceptableDistance;
        agent.SetDestination(target.transform.position);
        agent.isStopped = false;

        return BTNodeResult.InProgress;
    }

    private void BlackboardValuedChanged(BlackboardEntry entry)
    {
        //if the target is updated, updated our target variable.
        if(entry.GetKeyName() == targetKeyName)
        {
            entry.GetVal(out target);
        }
    }

    protected override BTNodeResult Update()
    {
        if(target == null) return BTNodeResult.Failure;

        if (InAcceptableDistance())
        {
            return BTNodeResult.Success;
        }

        agent.SetDestination(target.transform.position);

        return BTNodeResult.InProgress;
    }

    private bool InAcceptableDistance()
    {
        return Vector3.Distance(owner.transform.position, target.transform.position) <= acceptableDistance;
    }

    public override void End()
    {
        if (agent) agent.isStopped = true;
        if(blackboard)
        {
            blackboard.onBlackboardValueChanged -= BlackboardValuedChanged;
        }
        base.End();
    }
}
