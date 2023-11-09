using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName ="BehaviorTree/BehaviorTree")]
public class BehaviorTree : ScriptableObject
{
    [SerializeField]
    BTNode_Root rootNode;

    [SerializeField]
    List<BTNode> nodes;

    [SerializeField]
    Blackboard blackboard;

    BTNode currentNode;
    public Blackboard GetBlackBoard() { return  blackboard; }

    //getter or accssor for the nodes
    public List<BTNode> GetNodes() { return nodes; }

    bool stopped;
    public void PreConstruct()
    {
        if(!rootNode)
        {
            rootNode = CreateNode(typeof(BTNode_Root)) as BTNode_Root;
            SaveTree();
        }

        Construct(rootNode);
    }

    protected virtual void Construct(BTNode_Root root)
    {

    }

    public void AbortIfCurrentIsLower(int priority)
    {
        if(currentNode.GetPriority() > priority)
        {
            rootNode.End();
        }
    }
    public void Start()
    {
        SortTree();
        foreach(BTNode node in nodes)
        {
            node.onBecomeActive += CurrentNodeChanged;
        }
        stopped = false;
    }

    private void CurrentNodeChanged(BTNode node)
    {
        currentNode = node;
    }

    public void Update()
    {
        if(stopped) return;
        rootNode.UpdateNode();
    }

    public BTNode CreateNode(System.Type nodeType)
    {
        BTNode newNode = ScriptableObject.CreateInstance(nodeType) as BTNode;
        newNode.name = nodeType.Name;
        newNode.Init(this);
        nodes.Add(newNode);
        AssetDatabase.AddObjectToAsset(newNode, this);

        SaveTree();

        return newNode;
    }

    public void SaveTree()
    {
        SortTree();
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
    }

    public void RemoveNode(BTNode node)
    {
        nodes.Remove(node);
        //AssetDatabase.RemoveObjectFromAsset(node);
        SaveTree();
    }
    public void Stop()
    {
        stopped = true;
        rootNode.End();
    }
    public void SortTree()
    {
        foreach(BTNode node in nodes)
        {
            IBTNodeParent parent = node as IBTNodeParent;
            if(parent!=null)
            {
                parent.SortChildren();
            }
        }
        int priorityCounter = 0;
        Traverse(rootNode, (BTNode n) =>
        {
            n.SortPriority(ref priorityCounter);
        });
    }

    internal BehaviorTree CloneTree()
    {
        BehaviorTree clone = Instantiate(this);
        clone.rootNode = rootNode.CloneNode() as BTNode_Root;

        clone.nodes = new List<BTNode>();
        clone.blackboard = Instantiate(blackboard);

        Traverse(clone.rootNode, 
            (BTNode node) =>
        {
            clone.nodes.Add(node);
            node.Init(clone);
        }
        );

        return clone;
    }

    public void Traverse(BTNode node, System.Action<BTNode> visitor)
    {
        visitor(node);
        IBTNodeParent nodeAsParent = node as IBTNodeParent;
        if(nodeAsParent!=null)
        {
            foreach(BTNode child in nodeAsParent.GetChildren())
            {
                Traverse(child, visitor);
            }
        }
    }

    // () is the call operator.
    // [] is the subscription operator.
    // = - = * also operators.
}
