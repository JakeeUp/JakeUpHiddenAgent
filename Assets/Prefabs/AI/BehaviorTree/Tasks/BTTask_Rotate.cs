using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BTTask_Rotate : BTNode
{
    [SerializeField]
    string targetKeyName = "target";

    [SerializeField]
    float acceptableDistance = 2.0f;

    NavMeshAgent agent;
    GameObject owner;
    GameObject target;
    Blackboard blackboard;

    [SerializeField] float angleDegrees;
    [SerializeField] float acceptableOffset;
    [SerializeField] float turnSpeed;

    float delta;

    Transform mTransform;

    Quaternion goalRotation;

    protected override BTNodeResult Execute()
    {
        if (!GetBehaviorTree()) return BTNodeResult.Failure;

        blackboard = GetBehaviorTree().GetBlackBoard();
        if (blackboard == null) return BTNodeResult.Failure;

        if (!blackboard.GetBlackboarData("owner", out owner))
            return BTNodeResult.Failure;

        agent = owner.GetComponent<NavMeshAgent>();
        if (!agent) return BTNodeResult.Failure;

        if (!blackboard.GetBlackboarData(targetKeyName, out target))
            return BTNodeResult.Failure;

        goalRotation = Quaternion.AngleAxis(angleDegrees, Vector3.up) * owner.transform.rotation; 

        if(IsInAcceptableAngle())
        {
            return BTNodeResult.Success;
        }

        return BTNodeResult.InProgress;
    }


    private bool IsInAcceptableAngle()
    {
        return Quaternion.Angle(goalRotation, owner.transform.rotation) <= acceptableOffset;
    }
    protected override BTNodeResult Update()
    {
        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, goalRotation, turnSpeed * Time.deltaTime);
        if(IsInAcceptableAngle())
        {
            return BTNodeResult.Success;
        }
        return BTNodeResult.InProgress;
    }

    private bool InAcceptableDistance()
    {
        return Vector3.Distance(owner.transform.position, target.transform.position) <= acceptableDistance;
    }

  
}
