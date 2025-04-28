using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;



    private float shakeTime;
    private float shakeIntensity;

    // Update is called once per frame
    public void Awake()
    {
        if (Instance != null) { Destroy(Instance); } // 유일성을 보장하기 위해

        Instance = this;
    }

    public void OnShakeCamera(float shakeTime = 1.0f, float shakeIntensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine(ShakeByPosition());
        StartCoroutine(ShakeByPosition());


    }

    private IEnumerator ShakeByPosition()
    {
        Vector3 startPosition = transform.position;

        while (shakeTime > 0.0f)
        {
            float y = Random.Range(-1f, 1f);
            transform.position = startPosition + new Vector3(0f, y, 0f) * shakeIntensity;

            transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = startPosition;
    }
}
