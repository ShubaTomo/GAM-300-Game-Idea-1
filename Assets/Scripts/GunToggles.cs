using UnityEditor.Timeline;
using UnityEngine;

public class GunToggles : MonoBehaviour
{
    public Vector3 originalPos;
    public Quaternion originalRot;

    public Vector3 scopedPos;
    public Quaternion scopedRot;

    public bool isScoped;



    private void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
        scopedPos = new Vector3(0f, -0.358f, 0.545f);
        scopedRot = new Quaternion(0f, 0f, 0f, 0f);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            onScope();
            isScoped = true;
        }

        if (isScoped && Input.GetMouseButtonUp(1))
        {
            unScope();
            isScoped = false;
        }
    }

    private void onScope()
    {
        transform.localRotation = scopedRot;
        transform.localPosition = scopedPos;
    }

    private void unScope()
    {
        transform.localPosition = originalPos;
        transform.localRotation = originalRot;
    }
}
