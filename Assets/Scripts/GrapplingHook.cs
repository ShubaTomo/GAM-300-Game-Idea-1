using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public Transform grappleOrigin;

    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.E;

    [Header("Grappling")]
    public string grappleTag = "Parkour";
    public float maxGrappleDistance = 80f;
    public float grappleSpeed = 45f;
    public float grappleAcceleration = 100f;
    public float stopDistance = 2f;

    [Header("Visual")]
    public LineRenderer grappleLine;

    private Vector3 grapplePoint;
    private bool isGrappling;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        if (grappleLine != null)
        {
            grappleLine.positionCount = 0;
        }
    }

    private void Update()
    {
        // Press E to grapple
        if (Input.GetKeyDown(grappleKey))
        {
            if (!isGrappling)
                StartGrapple();
            else
                StopGrapple();
        }

        // Update grapple rope
        if (isGrappling)
        {
            UpdateGrappleLine();
        }
    }

    private void FixedUpdate()
    {
        if (isGrappling)
        {
            GrappleMovement();
        }
    }

    private void StartGrapple()
    {
        Ray ray = new Ray(
            cameraTransform.position,
            cameraTransform.forward
        );

        // Shoot ray from camera
        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            maxGrappleDistance
        ))
        {
            // Check for Parkour tag
            if (!hit.collider.CompareTag(grappleTag))
                return;

            // Store grapple point
            grapplePoint = hit.point;

            isGrappling = true;
            pm.grappling = true;

            // Disable gravity while grappling
            rb.useGravity = false;

            // Start rope
            if (grappleLine != null)
            {
                grappleLine.positionCount = 2;
            }
        }
    }

    private void GrappleMovement()
    {
        Vector3 directionToPoint =
            grapplePoint - transform.position;

        float distance =
            directionToPoint.magnitude;

        // Stop when close enough
        if (distance <= stopDistance)
        {
            StopGrapple();
            return;
        }

        directionToPoint.Normalize();

        Vector3 targetVelocity =
            directionToPoint * grappleSpeed;

        Vector3 velocityDifference =
            targetVelocity - rb.linearVelocity;

        // Pull player toward grapple point
        rb.AddForce(
            velocityDifference.normalized *
            grappleAcceleration,
            ForceMode.Acceleration
        );

        // Limit grapple speed
        if (rb.linearVelocity.magnitude >
            grappleSpeed)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized *
                grappleSpeed;
        }
    }

    private void StopGrapple()
    {
        isGrappling = false;
        pm.grappling = false;

        // Restore gravity
        rb.useGravity = true;

        // Remove rope
        if (grappleLine != null)
        {
            grappleLine.positionCount = 0;
        }
    }

    private void UpdateGrappleLine()
    {
        if (grappleLine == null)
            return;

        grappleLine.SetPosition(
            0,
            grappleOrigin.position
        );

        grappleLine.SetPosition(
            1,
            grapplePoint
        );
    }
}