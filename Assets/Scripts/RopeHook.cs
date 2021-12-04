using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeHook : MonoBehaviour
{
    [SerializeField] private SpringJoint2D ropeSpring;
    private LineRenderer lineRenderer;

    [SerializeField] private LayerMask ropeBlockingLayer;
    [SerializeField] private LayerMask hookableLayer;
    [SerializeField] private LayerMask collectableLayer;

    [Header("Sounds")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip shootAudioClip;
    [SerializeField] private AudioClip hookAttachedAudioClip;

    [Header("State")]
    [SerializeField] private bool isHooked;
    [SerializeField] private bool isShooting;

    [Header("Rope Properties")]
    [SerializeField] [Range(0, 1)] private float ropeDeploymentSpeed;
    [SerializeField] private float ropeMinLength;
    [SerializeField] private float ropeMaxLength;
    [SerializeField] private AnimationCurve ropeSpeedCurve;

    void Start()
    {
        isShooting = false;
        isHooked = false;
        ropeSpring = GetComponent<SpringJoint2D>();
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (IsHooked())
        {
            ShowRopeAt(GetAnchorPosition());
        }
    }

    public void TriggerHookShot(float aimAngle)
    {
        if (isShooting)
        {
            return;
        }

        isShooting = true;
        PlayShootSound();

        Vector2 direction = Quaternion.AngleAxis(aimAngle, Vector3.back) * Vector2.up;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, ropeMaxLength, ropeBlockingLayer);
        if (hit)
        {
            // check if hit layer is part of hookableLayer
            if (hookableLayer == (hookableLayer | (1 << hit.collider.gameObject.layer)))
            {
                StartCoroutine(DeployHookAndAttach(hit.point));
            }
            else if (collectableLayer == (collectableLayer | (1 << hit.collider.gameObject.layer)))
            {
                StartCoroutine(DeployHookAndPull(hit.point, hit.collider.gameObject));
            }
            // hit a ropeBlocking layer, which is not hookable
            else
            {
                StartCoroutine(DeployHookAndRecallImmediately(hit.point));
            }
        }
        // hit nothing
        else
        {
            Vector2 endOfRope = transform.position + Quaternion.AngleAxis(aimAngle, Vector3.back) * Vector2.up * ropeMaxLength;
            StartCoroutine(DeployHookAndRecallImmediately(endOfRope));
        }
    }

    private void ShowRopeAt(Vector3 target)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target);
    }

    private IEnumerator DeployHookAndAttach(Vector3 targetPosition, bool attachToPosition = false)
    {
        yield return StartCoroutine(DeployHook(targetPosition));

        // deployment finished now attach
        ropeSpring.enabled = true;
        ropeSpring.connectedAnchor = new Vector3(targetPosition.x, targetPosition.y);
        isHooked = true;
        ropeSpring.distance = Vector2.Distance(transform.position, GetAnchorPosition());
        PlayHookAttachedSound();
    }

    private IEnumerator DeployHookAndPull(Vector3 targetPosition, GameObject collectableObject)
    {
        yield return StartCoroutine(DeployHook(targetPosition));
        yield return StartCoroutine(RecallHook(targetPosition, collectableObject));

    }

    private IEnumerator DeployHookAndRecallImmediately(Vector3 targetPosition)
    {
        yield return StartCoroutine(DeployHook(targetPosition));
        yield return StartCoroutine(RecallHook(targetPosition));
    }

    private IEnumerator DeployHook(Vector3 targetPosition)
    {
        float deployDuration = 0;
        Vector3 intermediatePosition = Vector3.Lerp(transform.position, targetPosition, deployDuration);
        while (Vector3.Distance(targetPosition, intermediatePosition) > 0.1f)
        {
            // not attached yet
            float iterationTime = 0.01f;
            yield return new WaitForSeconds(iterationTime);
            deployDuration += ropeDeploymentSpeed;
            intermediatePosition = Vector3.Lerp(transform.position, targetPosition, deployDuration);
            ShowRopeAt(intermediatePosition);
        }
    }

    public IEnumerator RecallHook(GameObject collectableObject = null)
    {
        yield return StartCoroutine(RecallHook(GetAnchorPosition(), collectableObject));
    }

    public IEnumerator RecallHook(Vector3 fromTargetPosition, GameObject collectableObject = null)
    {
        float remainingRecallDuration = 1;
        Vector3 currentAnchorPosition = fromTargetPosition;
        DetachFromAnchor();

        Vector3 intermediatePosition = Vector3.Lerp(transform.position, currentAnchorPosition, remainingRecallDuration);
        while (Vector3.Distance(transform.position, intermediatePosition) > 0.1f)
        {
            float iterationTime = 0.01f;
            yield return new WaitForSeconds(iterationTime);
            remainingRecallDuration -= ropeDeploymentSpeed;
            intermediatePosition = Vector3.Lerp(transform.position, currentAnchorPosition, remainingRecallDuration);
            if (collectableObject != null)
            {
                collectableObject.GetComponent<Collectable>().SetPosition(intermediatePosition);
            }
            ShowRopeAt(intermediatePosition);
        }
        isShooting = false;
        lineRenderer.enabled = false;
    }

    public bool IsHooked()
    {
        return isHooked;
    }

    private Vector3 GetAnchorPosition()
    {
        return ropeSpring.connectedAnchor;
    }


    public void DetachFromAnchor()
    {
        ropeSpring.connectedBody = null;
        ropeSpring.enabled = false;
        isHooked = false;
    }

    public void ControlRope(LengthChange direction, float duration)
    {
        float distanceChange = ropeSpeedCurve.Evaluate(duration) * (int)direction;
        ropeSpring.distance = Mathf.Clamp(ropeSpring.distance - distanceChange, ropeMinLength, ropeMaxLength);
    }

    private void PlayShootSound()
    {
        audioSource.clip = shootAudioClip;
        audioSource.Play();
    }

    private void PlayHookAttachedSound()
    {
        audioSource.clip = hookAttachedAudioClip;
        audioSource.Play();
    }

    public enum LengthChange
    {
        EXTEND = 1, SHORTEN = -1
    }

}
