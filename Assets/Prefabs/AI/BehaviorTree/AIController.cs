using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] BehaviorTree behaviorTreeAsset;

    BehaviorTree behaviorTree;
    PerceptionComponent perceptionComponent;
    private void Awake()
    {
        perceptionComponent = GetComponent<PerceptionComponent>();
        if(perceptionComponent)
        {
            perceptionComponent.onTargetUpdated += TargetUpdated;
        }
    }

    private void TargetUpdated(GameObject newTarget)
    {
        if(newTarget == null)
        {
            if(behaviorTree.GetBlackBoard().GetBlackboarData("target", out GameObject target))
            {
                behaviorTree.GetBlackBoard().SetBloackboardData("lastSeenLocation", target.transform.position);
            }
        }

        behaviorTree.GetBlackBoard().SetBloackboardData("target", newTarget);
    }

    // Start is called before the first frame update
    void Start()
    {
        behaviorTree = behaviorTreeAsset.CloneTree();
        behaviorTree?.PreConstruct();
        behaviorTree.GetBlackBoard().SetBloackboardData("owner", gameObject);

        behaviorTree.Start();
    }

    public BehaviorTree GetBehaviorTree()
    {
        if(behaviorTree)
        {
            return behaviorTree;
        }

        return behaviorTreeAsset;
    }

    // Update is called once per frame
    void Update()
    {
        behaviorTree?.Update();
    }

    internal void StopAILogic()
    {
        behaviorTree.Stop();
    }
}
