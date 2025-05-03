using UnityEngine;

public class LaunchPadController : MonoBehaviour
{
    [Tooltip("The force applied to the player when launched.")]
    [SerializeField] private float launchForce = 20f;

    [Tooltip("The angle at which the player is launched (in degrees).")]
    [SerializeField] private float launchAngle = 45f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has a Rigidbody
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Reset the object's velocity
            rb.linearVelocity = Vector3.zero;

            // Calculate the launch direction
            Vector3 launchDirection = Quaternion.Euler(-launchAngle, 0, 0) * transform.forward;

            // Apply the launch force
            rb.AddForce(launchDirection.normalized * launchForce, ForceMode.VelocityChange);
        }
    }
}