using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log($"Killer entered {col.gameObject} {col.gameObject.tag}");
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<DeathHandler>().Die();
        }
    }
}
