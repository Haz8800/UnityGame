using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    [Header("Default Shake Settings")]
    public float defaultDuration = 0.3f;
    public float defaultMagnitude = 0.3f;

    private Vector3 originalPosition;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        if (duration <= 0) duration = defaultDuration;
        if (magnitude <= 0) magnitude = defaultMagnitude;

        StopAllCoroutines();
        StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * magnitude;
            transform.localPosition = originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
