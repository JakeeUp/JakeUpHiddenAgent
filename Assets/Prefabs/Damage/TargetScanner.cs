using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScanner : MonoBehaviour
{
    float range;
    float duration;
    [SerializeField] Transform scanPivot;
    public event Action<GameObject> onNewTargetFound;

    private void Awake()
    {
        scanPivot.transform.localScale = Vector3.zero;
    }
    public void Init(float range, float duration, GameObject Visual)
    {
        this.range = range;
        this.duration = duration;
        if(Visual != null)
        {
            Visual.transform.parent = scanPivot;
            Visual.transform.localPosition = Vector3.zero;
        }
        
    }
    public void SetupAttachment(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
    }
    public void StartScan()
    {
        StartCoroutine(ScanCoroutine());
    }
    private void OnTriggerEnter(Collider other)
    {
        onNewTargetFound?.Invoke(other.gameObject);
    }

    IEnumerator ScanCoroutine()
    {
        float timeElapsed = 0;
        float scaleRate = range / duration;
        while(timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            scanPivot.transform.localScale += Vector3.one * scaleRate * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }


}
