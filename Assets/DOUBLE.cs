using UnityEngine;

public class DoubleShotgunController : MonoBehaviour
{
    public Shotgun rightShotgun;
    public Shotgun leftShotgun;

    private bool shootRightNext = true;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    void Fire()
    {
        if (shootRightNext)
        {
            if (rightShotgun != null)
                rightShotgun.ForceFire();
        }
        else
        {
            if (leftShotgun != null)
                leftShotgun.ForceFire();
        }

        shootRightNext = !shootRightNext; // Alternate after each shot
    }
}
