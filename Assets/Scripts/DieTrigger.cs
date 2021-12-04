using UnityEngine;

public class DieTrigger : MonoBehaviour
{
    public void TriggerDieInParent()
    {
        transform.GetComponentInParent<PlayerController>().Die();
    }

}
