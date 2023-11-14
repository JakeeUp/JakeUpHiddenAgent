using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementInterface 
{
    public void RotateTowards(Vector3 direction);
    public void RotateTowards(GameObject target);

    public float GetMoveSpeed();

    public void SetMoveSpeed(float speed);  
}
