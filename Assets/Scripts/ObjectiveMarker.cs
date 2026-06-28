using TMPro;
using UnityEngine;

public class ObjectiveMarker : MonoBehaviour
{
    public Transform target;
    public Camera playerCamera;

    private RectTransform rect;
    private TMP_Text markerText;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        markerText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector3 screenPos =
            playerCamera.WorldToScreenPoint(
                target.position);

        if (screenPos.z > 0)
        {
            markerText.enabled = true;
            rect.position = screenPos;
        }
        else
        {
            markerText.enabled = false;
        }
    }
}