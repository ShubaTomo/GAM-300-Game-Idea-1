using UnityEngine;

public class AimDownSight : MonoBehaviour
{
    public Transform hipPosition;
    public Transform aimPosition;

    public float moveSpeed = 10f;

    private void Update()
    {
        bool aiming =
            Input.GetMouseButton(1);

        Transform target =
            aiming
            ? aimPosition
            : hipPosition;

        transform.localPosition =
            Vector3.Lerp(
                transform.localPosition,
                target.localPosition,
                Time.deltaTime * moveSpeed);

        transform.localRotation =
            Quaternion.Lerp(
                transform.localRotation,
                target.localRotation,
                Time.deltaTime * moveSpeed);
    }

    public void SnapToAim()
    {
        transform.localPosition =
            aimPosition.localPosition;

        transform.localRotation =
            aimPosition.localRotation;
    }
}