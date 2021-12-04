using UnityEngine;

public class DieTrigger : MonoBehaviour
{
    public void TriggerDieInParent()
    {
        Debug.Log("Die");
        transform.GetComponentInParent<PlayerController>().Die();
    }

}
