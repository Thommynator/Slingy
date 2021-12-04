using UnityEngine;
using PathCreation;

public class PathFollower : MonoBehaviour
{

    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private float speed;
    [SerializeField] private bool randomSpeed;

    private float traveledDistance;

    void Start()
    {
        if (randomSpeed)
        {
            speed = Random.Range(-2.0f, 2.0f);
        }
        traveledDistance = Random.Range(0, 10);
    }
    void Update()
    {
        traveledDistance += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(traveledDistance);
    }
}
