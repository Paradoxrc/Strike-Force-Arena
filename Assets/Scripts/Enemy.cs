using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Variables related to firing and visual/audio effects
    public GameObject bulletPrefab;             // The bullet prefab the enemy will shoot
    public Transform bulletPosition;            // The position from which the bullet will be fired
    public GameObject bulletFiringEffect;       // The visual effect for bullet firing
    public AudioClip enemyShootingAudio;        // The sound the enemy makes when shooting
    public float fireRate = 0.5f;               // Rate at which the enemy can shoot
    public float moveSpeed = 2.0f;              // Speed at which the enemy moves towards the player
    public float detectionRange = 15.0f;        // Distance within which the enemy starts following the player
    public float stopRange = 5.0f;              // Distance at which the enemy stops moving closer to the player

    public int health = 100;
    public Slider healthBar;

    private Transform player;                   // Reference to the player's transform
    private float nextFire = 0;                 // Time management for firing bullets

    public delegate void EnemyKilled();
    public static event EnemyKilled OnEnemyKilled;

    void Start()
    {
        // Find the player object by tag (ensure the player has the tag "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Check if the player is within detection range but not too close
            if (distanceToPlayer <= detectionRange && distanceToPlayer > stopRange)
            {
                // Move towards the player
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.LookAt(player);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            // Rotate the enemy to face the player
            transform.LookAt(other.transform);

            // Fire at the player
            Fire();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();
            TakeDamage(bullet.damage);
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.value = health;
        if (health <= 0)
            EnemyDied();
    }

    void EnemyDied()
    {
        gameObject.SetActive(false);

        if (OnEnemyKilled != null)
            OnEnemyKilled.Invoke();
    }

    private void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
            bullet.GetComponent<BulletController>()?.InitializeBullet(transform.rotation * Vector3.forward);

            AudioManager.Instance?.Play3D(enemyShootingAudio, transform.position);
            VFXManager.Instance?.PlayVFX(bulletFiringEffect, bulletPosition.position);

            print("Enemy is firing");
        }
    }
}
