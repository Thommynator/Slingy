using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private List<AudioClip> eatingAudioClips;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private bool isCollected;
    private Collector collector;
    private PathFollower pathFollower;

    void Start()
    {
        isCollected = false;
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        LeanTween.scale(this.gameObject, Vector3.one * 1.4f, 1).setLoopPingPong();
        LeanTween.rotateZ(this.gameObject, 5, 2).setFrom(-5).setLoopPingPong();
        collector = GameObject.Find("Collector").GetComponent<Collector>();
        pathFollower = GetComponent<PathFollower>();
    }

    public void SetPosition(Vector3 position)
    {
        pathFollower.enabled = false;
        transform.position = position;
    }

    private void PlayEatSound()
    {
        audioSource.clip = eatingAudioClips[Random.Range(0, eatingAudioClips.Count)];
        audioSource.Play();
    }

    private void InformCollector()
    {
        if (collector != null)
        {
            collector.Collect();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isCollected && collider.gameObject.tag == "Player")
        {
            isCollected = true;
            LeanTween.cancel(this.gameObject);
            LeanTween.scale(this.gameObject, Vector3.one, 1);
            spriteRenderer.enabled = false;
            GetComponent<PolygonCollider2D>().enabled = false;
            PlayEatSound();
            InformCollector();
            Destroy(this.transform.parent.gameObject, 0.5f);
        }
    }


}
