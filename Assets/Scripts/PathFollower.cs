using UnityEngine;
using PathCreation;

public class PathFollower : MonoBehaviour
{

    [SerializeField] private PathCreator pathCreator;

    private float speed;

    private float traveledDistance;

    void Start()
    {
        speed = Random.Range(-2.0f, 2.0f);
        traveledDistance = Random.Range(0, 10);
    }
    void Update()
    {
        traveledDistance += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(traveledDistance);
    }
}
