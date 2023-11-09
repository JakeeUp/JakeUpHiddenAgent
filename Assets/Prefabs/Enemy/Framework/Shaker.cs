using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeManginatude = 2f;
    [SerializeField] bool shaking;

    public void StartShake()
    {
        if(!shaking)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }

    IEnumerator ShakeCoroutine()
    {
        shaking = true;
        yield return new WaitForSeconds(shakeDuration);
        shaking = false;
    }

    private void LateUpdate()
    {
        if(shaking)
        {
            Vector3 shakeAmt = new Vector3(Random.value, Random.value, Random.value) * shakeManginatude * (Random.value > 0.5 ? 1 : -1);
            transform.localPosition += shakeAmt;
        }
        else
        {
            transform.localPosition += Vector3.zero;

        }
    }
}
