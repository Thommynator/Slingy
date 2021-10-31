using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeHook : MonoBehaviour
{
    [SerializeField] private SpringJoint2D rope;
    private LineRenderer lineRenderer;

    [SerializeField] private LayerMask ropeBlockingLayer;
    [SerializeField] private LayerMask hookableLayer;

    [Header("State")]
    [SerializeField] private bool isHooked;
    [SerializeField] private bool isShooting;

    [Header("Rope Properties")]
    [SerializeField] private float ropeMinLength;
    [SerializeField] private float ropeMaxLength;
    [SerializeField] private float ropeSpeed;

    void Start()
    {
        isShooting = false;
        isHooked = false;
        rope = GetComponent<SpringJoint2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (IsHooked())
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, GetHookPosition());
        }
    }

    public void ShootHook(float aimAngle)
    {
        if (isShooting)
        {
            return;
        }

        BreakHook();
        isShooting = true;

        Vector2 direction = Quaternion.AngleAxis(aimAngle, Vector3.back) * Vector2.up;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, ropeMaxLength, ropeBlockingLayer);
        if (hit)
        {
            // check if hit layer is part of hookableLayer
            if (hookableLayer == (hookableLayer | (1 << hit.collider.gameObject.layer)))
            {
                rope.enabled = true;
                rope.connectedBody = hit.rigidbody;
                rope.connectedAnchor = new Vector3(hit.point.x, hit.point.y) - hit.transform.position;
                rope.distance = Vector2.Distance(transform.position, GetHookPosition());
                isHooked = true;
            }
            // hit a ropeBlocking layer, which is not hookable
            else
            {
                StartCoroutine(ShowMissedShot(hit.point, 0.2f));
            }
        }
        // hit nothing
        else
        {
            Vector2 endOfRope = transform.position + Quaternion.AngleAxis(aimAngle, Vector3.back) * Vector2.up * ropeMaxLength;
            StartCoroutine(ShowMissedShot(endOfRope, 0.2f));
        }

        isShooting = false;

    }

    private IEnumerator ShowMissedShot(Vector2 targetPosition, float duration)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, targetPosition);
        yield return new WaitForSeconds(duration);
        lineRenderer.enabled = false;
    }

    public bool IsHooked()
    {
        return isHooked;
    }

    private Vector3 GetHookPosition()
    {
        return (Vector2)rope.connectedBody.transform.position + rope.connectedAnchor;
    }

    public void BreakHook()
    {
        lineRenderer.enabled = false;
        rope.connectedBody = null;
        rope.enabled = false;
        isHooked = false;
    }

    public void ControlRope(float distanceChange)
    {
        rope.distance = Mathf.Clamp(rope.distance - distanceChange * ropeSpeed, ropeMinLength, ropeMaxLength);
    }

}
