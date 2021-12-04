using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private float aimAngle;
    private RopeHook ropeHook;

    [Header("Sounds")]
    private AudioSource audioSource;
    private AudioSource explosionSoundSource;
    [SerializeField] private List<AudioClip> jumpAudioClips;
    [SerializeField] private List<AudioClip> bounceAudioClips;
    [SerializeField] private List<AudioClip> explosionAudioClips;


    [Header("Player")]
    private Rigidbody2D body;
    [SerializeField] private float maxRotationForce;
    [SerializeField] private float aimRotationSpeed;
    [SerializeField] private GameObject bloodExplosionParticlesPrefab;
    float changeRopeDuration;
    private Animator animator;
    private bool canShoot;



    void Start()
    {
        transform.position = spawnPosition.position;
        body = GetComponent<Rigidbody2D>();
        ropeHook = GetComponent<RopeHook>();
        audioSource = GetComponent<AudioSource>();
        explosionSoundSource = transform.Find("ExplosionSoundObject").GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        canShoot = true;
    }

    void Update()
    {
        crosshair.transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.back);

        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            if (ropeHook.IsHooked())
            {
                animator.SetTrigger("JumpRoll");
                PlayRandomJumpSound();
                StartCoroutine(ropeHook.RecallHook());
            }
            else
            {
                ropeHook.TriggerHookShot(aimAngle);
            }
        }

        animator.SetFloat("VerticalSpeed", body.velocity.y);
        if (body.velocity.x >= 1f)
        {
            transform.Find("Sprite").localScale = new Vector3(1, 1, 1);
        }
        else if (body.velocity.x <= -1f)
        {
            transform.Find("Sprite").localScale = new Vector3(-1, 1, 1);
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
            if (Input.GetAxis("Horizontal") != 0)
            {
                SwingBodyAtRope();
            }

            if (Input.GetKey(KeyCode.S))
            {
                ropeHook.ControlRope(RopeHook.LengthChange.SHORTEN, changeRopeDuration);
                changeRopeDuration += Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                ropeHook.ControlRope(RopeHook.LengthChange.EXTEND, changeRopeDuration);
                changeRopeDuration += Time.deltaTime;
            }
            else
            {
                changeRopeDuration = 0;
            }
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

    public void Respawn()
    {
        canShoot = false;
        ropeHook.ResetHook();
        animator.SetTrigger("Die");
    }

    public void Die()
    {
        GameObject.Instantiate(bloodExplosionParticlesPrefab, transform.position, Quaternion.identity);
        PlayRandomExplosionSound();
        LeanTween.scale(transform.Find("Sprite").gameObject, Vector3.zero, 0.5f).setOnComplete(() =>
        {
            transform.position = spawnPosition.position;
            LeanTween.scale(transform.Find("Sprite").gameObject, Vector3.one, 0.3f).setOnComplete(() => canShoot = true);
        });
    }

    private void PlayRandomJumpSound()
    {
        audioSource.clip = jumpAudioClips[Random.Range(0, jumpAudioClips.Count)];
        audioSource.Play();
    }

    private void PlayRandomBounceSound()
    {
        audioSource.clip = bounceAudioClips[Random.Range(0, bounceAudioClips.Count)];
        audioSource.Play();
    }

    private void PlayRandomExplosionSound()
    {
        explosionSoundSource.clip = explosionAudioClips[Random.Range(0, explosionAudioClips.Count)];
        explosionSoundSource.Play();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayRandomBounceSound();
    }

}
