using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Finish : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (GameManager.current.CanFinishLevel())
        {
            Debug.Log(LayerMask.LayerToName(collider.gameObject.layer));
            if (collider.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
            {
                GameManager.current.NextLevel();
            }
        }
    }
}
