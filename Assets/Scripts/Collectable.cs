using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    [SerializeField] private Color startColor;
    [SerializeField] private Color collectedColor;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private bool isCollected;


    void Start()
    {
        isCollected = false;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = startColor;
        audioSource = GetComponent<AudioSource>();
    }

    private void ChangeColor()
    {
        spriteRenderer.color = collectedColor;
    }

    private void PlaySound()
    {
        audioSource.Play();
    }

    private void InformCollector()
    {
        transform.parent.parent.TryGetComponent<Collector>(out Collector collector);
        if (collector != null)
        {
            collector.Collect();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isCollected)
        {
            isCollected = true;
            ChangeColor();
            PlaySound();
            InformCollector();
        }
    }


}
