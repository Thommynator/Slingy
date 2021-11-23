using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraZoomOnVelocity : MonoBehaviour
{

    [SerializeField] private float minOrthographicSize;
    [SerializeField] private float maxOrthographicSize;
    [SerializeField] private float minObjectSpeed;
    [SerializeField] private float maxObjectSpeed;
    [SerializeField] private float lerpFactor;

    private CinemachineVirtualCamera cinemachineVirtualCamera;




    // Start is called before the first frame update
    void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        GameObject player = cinemachineVirtualCamera.m_Follow.gameObject;
        float playerVelocity = player.GetComponent<Rigidbody2D>().velocity.magnitude;
        float currentOrthoSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        float targetOrthoSize = map(playerVelocity, minObjectSpeed, maxObjectSpeed, minOrthographicSize, maxOrthographicSize);
        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthoSize, lerpFactor);
    }

    private float map(float value, float minFrom, float maxFrom, float minTo, float maxTo)
    {
        return minTo + (maxTo - minTo) * ((value - minFrom) / (maxFrom - minFrom));
    }
}
