using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDie : MonoBehaviour
{
    [Header("Configuration")]
    public bool DieOutOfViewport = true;

    [SerializeField]
    private Vector3 RelPos;

    private void Die()
    {
        if(gameObject.name == "Player") {
            Game.obj.EndGame();
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        RelPos = Game.obj.Camera.WorldToViewportPoint(transform.position - new Vector3(0, -1.5f));
        if (DieOutOfViewport && RelPos.y < Game.DeathYPosition)
        {
            Die();
        }
    }
}
