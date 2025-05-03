using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180f, 0); // Flip to face camera
        transform.position += Vector3.up * Time.deltaTime * 0.5f; // float upward
    }

    void Start()
    {
        Destroy(gameObject, 1.2f); // auto-destroy
    }
}
