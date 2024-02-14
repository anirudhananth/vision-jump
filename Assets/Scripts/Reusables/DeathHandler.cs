using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [Header("Configuration")]
    public bool DieOutOfViewport = true;
    public AudioClip deathAudio;

    [SerializeField]
    private Vector3 RelPos;
    private bool isDead;
    private AudioSource audioSource;

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        if (audioSource != null && deathAudio != null)
        {
            Utils.Play(audioSource, deathAudio);
        }
        if(gameObject.name == "Player") {
            Game.obj.EndGame();
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().velocity *= 0.1f;
            var animator = GetComponentInChildren<Animator>();
            if (animator != null) {
                animator.SetTrigger("die");
                GetComponentInChildren<ParticleSystem>().Play();
            }
            Destroy(gameObject, 0.5f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
