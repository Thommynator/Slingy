using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collector : MonoBehaviour
{

    [SerializeField] List<GameObject> collectableObjects;

    [SerializeField] private TextMeshProUGUI collectedNumberText;
    private int collectedNumber;


    void Start()
    {
        collectedNumber = 0;
        foreach (Transform child in transform.Find("Collectables").transform)
        {
            collectableObjects.Add(child.gameObject);
        }
        UpdateScore(collectedNumber, collectableObjects.Count);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Collect()
    {
        collectedNumber++;
        UpdateScore(collectedNumber, collectableObjects.Count);
        GameManager.current.CanFinishLevel(collectedNumber == collectableObjects.Count);
    }

    private void UpdateScore(int collectedNumber, int totalNumber)
    {
        collectedNumberText.text = string.Format("{0} / {1}", collectedNumber, totalNumber);
    }
}
