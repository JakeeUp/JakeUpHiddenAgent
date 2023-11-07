using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public enum BlackboardType
{
    Int,
    Float,
    Bool,
    Vector3,
    GameObject
}

[Serializable]
public class BlackboardEntry
{
    [SerializeField] string keyName;
    [SerializeField] object value;
    [SerializeField] string runtimeValue;
    [SerializeField] BlackboardType type;

    public string GetKeyName() { return keyName; }
    public BlackboardEntry(string keyname, BlackboardType type)
    {
        this.keyName = keyname;
        this.value = null;
        this.runtimeValue = "null";
        this.type = type;
    }

    //if val is null, clears the data.
    public bool SetVal<T>(T val)
    {
        if(val == null)
        {
            value = null;
            runtimeValue = "null";
            return true; 
        }

        if(val.GetType() != typeDictionary[type])
        {
            Debug.LogError($"trying to set blackboard data: {keyName}, with type {val.GetType()}, should be: {typeDictionary[type]}");
            return false;
        }

        value = val;
        runtimeValue = val.ToString();
        return true;
    }

    public bool GetVal<T>(out T val)
    {
        val = default;
        if (value == null) return false;

        if(typeof(T) != typeDictionary[type])
        {
            return false;
        }

        val = (T)value; //casting is safe here, because we have guarenteed that the type T is the type of the value member variable.
        return true;
    }

    private static Dictionary<BlackboardType, System.Type> typeDictionary = new Dictionary<BlackboardType, Type>
    {
        {BlackboardType.Float, typeof(float)},
        {BlackboardType.Int, typeof(int)},
        {BlackboardType.Bool, typeof(bool)},
        {BlackboardType.Vector3 , typeof(Vector3)},
        {BlackboardType.GameObject, typeof(GameObject)}
    };

    internal object GetRawValue()
    {
        return value;
    }

    public void ClearEntryValue()
    { 
        value = null;
        runtimeValue = "";
    }
}
[CreateAssetMenu(menuName = "BehaviorTree/Blackboard")]
public class Blackboard : ScriptableObject
{
    [SerializeField]
    List<BlackboardEntry> blackboardData;

    public delegate void OnBloackboardValueChanged(BlackboardEntry entry);
    public event OnBloackboardValueChanged onBlackboardValueChanged;

    public bool SetBloackboardData<T>(string keyName, T val)
    {
        foreach(var entry in blackboardData)
        {
            if(entry.GetKeyName() == keyName)
            {
                if(entry.SetVal(val))
                {
                    onBlackboardValueChanged?.Invoke(entry);
                    return true;
                }
                else
                {
                    return false; 
                }
            }
        }

        return false;
    }

    public bool GetBlackboarData<T>(string keyName, out T val)
    {
        foreach(var entry in blackboardData)
        {
            if(entry.GetKeyName() == keyName)
            {
                return entry.GetVal(out val);
            }
        }
        val = default;
        return false;
    }

    internal object GetBlackboarRawData(string keyName)
    {
        foreach (var entry in blackboardData)
        {
            if (entry.GetKeyName() == keyName)
            {
                return entry.GetRawValue();
            }
        }

        return null;
    }

    public void ClearBlackboardData(string keyName)
    {
        foreach(var entry in blackboardData)
        {
            if (entry.GetKeyName() == keyName)
            {
                entry.ClearEntryValue();
            }
        }
    }
}
