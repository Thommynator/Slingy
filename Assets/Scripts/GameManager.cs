using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager current;
    [SerializeField] private int maxTravelDistance;
    [SerializeField] private GameObject cinemachine;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerPrefab;


    void Start()
    {
        current = this;
        player = GameObject.FindGameObjectWithTag("Player");
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


}
