using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    float xRotation;
    float yRotation;

    private bool hasFallen = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (GameManagerScript.isDead)
        {
            if (!hasFallen)
            {
                FallToFloor();
                hasFallen = true;
            }
            return;
        }
        else
        {
            hasFallen = false;
        }

        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }

    /// <summary>
    /// Makes the camera fall to the floor when the player dies.
    /// </summary>
    public void FallToFloor()
    {
        // Animate the camera holder to "fall" to the floor over 0.7 seconds.
        // You may adjust the Y value (-1f) to match your game's floor height.
        camHolder.DOLocalRotate(new Vector3(90f, yRotation, 0f), 0.7f).SetEase(Ease.InQuad);
        camHolder.DOLocalMoveY(-1f, 0.7f).SetRelative(true).SetEase(Ease.InQuad);
    }
}