using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] float turnSpeed = 30f;
    public float RotateTowards(Vector3 lookDir)
    {
        float rotationSpeed = 0f;
        if (lookDir.magnitude != 0)
        {
            Quaternion prevRot = transform.rotation; // before rotate
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir, Vector3.up), Time.deltaTime * turnSpeed);
            Quaternion newRot = transform.rotation; // after rotote

            float rotationDelta = Quaternion.Angle(prevRot, newRot); // how much whe have rotated in this frame.

            float rotateDir = Vector3.Dot(lookDir, transform.right) > 0 ? 1 : -1;

            rotationSpeed = rotationDelta / Time.deltaTime * rotateDir;
        }

        return rotationSpeed;
    }
}
