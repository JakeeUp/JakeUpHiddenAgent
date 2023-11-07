using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_Attack : BTNode
{
    [SerializeField] string keyName;

    protected override BTNodeResult Execute()
    {
        IBTTaskInterface taskInterface = GetInterface();
        if(taskInterface == null )
        {
            return BTNodeResult.Failure;
        }

        Blackboard blackboard = GetBlackboard();
        if(!blackboard)
            return BTNodeResult.Failure;

        blackboard.GetBlackboarData(keyName, out GameObject target);

        taskInterface.AttackTarget(target);

        return BTNodeResult.Success;

    }
}
