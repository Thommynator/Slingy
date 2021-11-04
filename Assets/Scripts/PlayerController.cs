using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private GameObject crosshair;
    [SerializeField] private float aimAngle;
    private RopeHook ropeHook;
    private AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField] private List<AudioClip> jumpAudioClips;


    [Header("Player")]
    private Rigidbody2D body;
    [SerializeField] private float maxRotationForce;
    [SerializeField] private float aimRotationSpeed;
    [SerializeField] private GameObject bloodExplosionParticlesPrefab;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        ropeHook = GetComponent<RopeHook>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        crosshair.transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.back);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ropeHook.IsHooked())
            {
                PlayRandomJumpSound();
                ropeHook.BreakHook();
            }
            else
            {
                ropeHook.ShootHook(aimAngle);
            }
        }
    }


    void FixedUpdate()
    {
        if (!ropeHook.IsHooked())
        {
            Aim();
        }
        else
        {
            SwingBodyAtRope();
            ropeHook.ControlRope(Input.GetAxis("Vertical"));
        }
    }


    private void Aim()
    {
        aimAngle = (aimAngle + Input.GetAxis("Horizontal") * aimRotationSpeed) % 360;
    }

    private void SwingBodyAtRope()
    {
        body.AddForce(Vector2.right * Input.GetAxis("Horizontal") * maxRotationForce);
    }

    public void Die()
    {
        GameObject.Instantiate(bloodExplosionParticlesPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void PlayRandomJumpSound()
    {
        audioSource.clip = jumpAudioClips[Random.Range(0, jumpAudioClips.Count)];
        audioSource.Play();
    }

}
