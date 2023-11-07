using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BTNode;

public enum BTNodeResult
{
    Success,
    InProgress,
    Failure
}

public enum BTNodePortType
{
    None,
    Single,
    Multi
}

public abstract class BTNode : ScriptableObject
{
    public delegate void OnNodeStateChanged(BTNodeResult newState);
    public event OnNodeStateChanged onNodeStateChanged;
    bool isStarted = false;

    [SerializeField]
    [HideInInspector]
    Vector2 graphPos;

    [SerializeField]
    [HideInInspector]
    string guid = "";

    [SerializeField]
    int priority;

    BehaviorTree owningBehaviorTree;
    public Action<BTNode> onBecomeActive;
    public int GetPriority() { return priority; }
    public void SortPriority(ref int priorityCount)
    {
        priority = priorityCount++;
    }
    public Blackboard GetBlackboard()
    {
        if(GetBehaviorTree())
        {
            return GetBehaviorTree().GetBlackBoard();
        }
        return null;
    }

    public GameObject GetOwner()
    {
        if(GetBlackboard())
        {
            GetBlackboard().GetBlackboarData("owner", out GameObject owner);
            return owner;
        }    
        return null;
    }

    IBTTaskInterface GetInterface()
    {
        GameObject owner = GetOwner();
        if(owner)
        {
            return owner.GetComponent<IBTTaskInterface>();
        }
        return null;
    }
    public void Init(BehaviorTree behaviorTree)
    {
        owningBehaviorTree = behaviorTree;
    }

    public BehaviorTree GetBehaviorTree()
    {
        return owningBehaviorTree;
    }

    public virtual BTNodePortType GetInputPortType()
    {
        return BTNodePortType.Single;
    }

    public virtual BTNodePortType GetOutputPortType()
    {
        return BTNodePortType.None;
    }


    //UpdateNode will be called by an update function in a monobehavior in the future.
    public BTNodeResult UpdateNode()
    {
        if(!isStarted)
        {
            onBecomeActive?.Invoke(this);
            BTNodeResult executeResult = Execute();
            onNodeStateChanged?.Invoke(executeResult);
            isStarted = true;
            //if not in progess, we have either failed or successed.
            if(executeResult != BTNodeResult.InProgress)
            {
                End();
                return executeResult;
            }
        }

        BTNodeResult updateResult = Update();
        onNodeStateChanged?.Invoke(updateResult);

        if (updateResult != BTNodeResult.InProgress)
        {
            End();
        }
        return updateResult;
    }

    public virtual void End()
    {
        isStarted = false;
    }

    protected virtual BTNodeResult Update()
    {
        return BTNodeResult.Success;
    }

    protected virtual BTNodeResult Execute()
    {
        return BTNodeResult.Success;
    }

    public void SetGraphPosition(Vector2 newPos)
    {
        graphPos = newPos;
    }

    public Vector2 GetGraphPosition() 
    { 
        return graphPos;
    }

    public virtual bool Contains(BTNode node)
    {
        return this == node;
    }

    public string GetGUID()
    {
        if(guid == "")
        {
            guid = GUID.Generate().ToString();
        }

        return guid;
    }

    public virtual BTNode CloneNode()
    {
        return Instantiate(this);
    }
}
