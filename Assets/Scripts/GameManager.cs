using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager current;
    [SerializeField] private int maxTravelDistance;
    [SerializeField] private GameObject cinemachine;
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;
    private bool canFinishLevel;


    void Start()
    {
        current = this;
        player = GameObject.FindGameObjectWithTag("Player");
        canFinishLevel = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }

        if (Vector2.Distance(player.transform.position, Vector2.zero) > maxTravelDistance)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Die();
        player = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        cinemachine.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
    }

    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int maxScenes = SceneManager.sceneCountInBuildSettings;
        Debug.Log("Next " + nextSceneIndex + " total " + maxScenes);
        if (nextSceneIndex < maxScenes)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("You reached the end!");
        }
    }

    public void CanFinishLevel(bool value)
    {
        canFinishLevel = value;
    }

    public bool CanFinishLevel()
    {
        return canFinishLevel;
    }


}
