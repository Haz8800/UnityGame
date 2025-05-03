using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    public float floatSpeed = 20f;
    public float lifetime = 1.5f;

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
