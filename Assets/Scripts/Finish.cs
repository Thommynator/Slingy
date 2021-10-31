using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Finish : MonoBehaviour
{

    [SerializeField] private UnityEvent onCollideFinish;

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(LayerMask.LayerToName(collider.gameObject.layer));
        if (collider.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            Debug.Log("Finish");
            onCollideFinish?.Invoke();
        }
    }
}
