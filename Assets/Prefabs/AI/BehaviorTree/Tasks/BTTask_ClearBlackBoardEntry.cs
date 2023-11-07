using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_ClearBlackBoardEntry : BTNode
{
    [SerializeField] string blackboardKeyName;


    protected override BTNodeResult Execute()
    {
        if (!GetBehaviorTree()) return BTNodeResult.Failure;

        Blackboard blackboard = GetBehaviorTree().GetBlackBoard();
        if (!blackboard) return BTNodeResult.Failure;


        blackboard.ClearBlackboardData(blackboardKeyName);
        return BTNodeResult.Success;
    }

}
