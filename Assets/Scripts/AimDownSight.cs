using UnityEngine;

public class AimDownSight : MonoBehaviour
{
    [Header("Gun Positions")]
    public Transform hipPosition;
    public Transform aimPosition;

    public float moveSpeed = 10f;

    [Header("Camera Zoom")]
    public Camera playerCamera;

    public float normalFOV = 60f;
    public float aimFOV = 40f;
    public float zoomSpeed = 10f;

    private void Update()
    {
        bool aiming =
            Input.GetMouseButton(1);

        Transform target =
            aiming
            ? aimPosition
            : hipPosition;

        // Move gun
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

        // Zoom camera
        float targetFOV =
            aiming
            ? aimFOV
            : normalFOV;

        playerCamera.fieldOfView =
            Mathf.Lerp(
                playerCamera.fieldOfView,
                targetFOV,
                Time.deltaTime * zoomSpeed);
    }

    public void SnapToAim()
    {
        transform.localPosition =
            aimPosition.localPosition;

        transform.localRotation =
            aimPosition.localRotation;
    }
}