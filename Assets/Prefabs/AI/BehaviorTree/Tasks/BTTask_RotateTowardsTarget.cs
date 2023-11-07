using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_RotateTowardsTarget : BTNode
{
    [SerializeField] string keyName;
    [SerializeField] float acceptableAngle;

    GameObject target;
    GameObject owner;
    Blackboard blackboard;
    IBTTaskInterface taskInterface;

    protected override BTNodeResult Execute()
    {
        if (!GetBehaviorTree()) return BTNodeResult.Failure;

        Blackboard blackboard = GetBehaviorTree().GetBlackBoard();
        if (!blackboard) return BTNodeResult.Failure;

        if(!blackboard.GetBlackboarData("owner", out owner))
            return BTNodeResult.Failure;
        
        
        if(!blackboard.GetBlackboarData(keyName, out target))
            return BTNodeResult.Failure;

        taskInterface = owner.GetComponent<IBTTaskInterface>();
        if (taskInterface == null)
            return BTNodeResult.Failure;

        if (InAcceptableAngle())
        {
            return BTNodeResult.Success;
        }

        return BTNodeResult.InProgress;

    }
    protected override BTNodeResult Update()
    {
        if(InAcceptableAngle())
        {
            return BTNodeResult.Success;
        }
        taskInterface.RotateTowards(target);
        return BTNodeResult.InProgress;
    }
    private bool InAcceptableAngle()
    {
        return Vector3.Angle(owner.transform.forward, (target.transform.position - owner.transform.position)) < acceptableAngle; 
    }
}
