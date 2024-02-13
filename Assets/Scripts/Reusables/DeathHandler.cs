using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDie : MonoBehaviour
{
    [Header("Configuration")]
    public bool DieOutOfViewport = true;

    [SerializeField]
    private Vector3 RelPos;
    private int health = 4;

    private void Die()
    {
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

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Enemy") {
            Destroy(col.gameObject);
            if(health-- == 0) {
                Debug.Log("Game Over.");
                Die();
            }
        }
    }
}
