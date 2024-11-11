using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Pun.UtilityScripts; // Import this for AddScore functionality
using Photon.Realtime;

public class MultiplayerBulletController : MonoBehaviourPunCallbacks
{
    Rigidbody rigidBody;

    public float bulletSpeed = 15f;
    public GameObject bulletImpactEffect;
    public AudioClip BulletHitAudio;
    public int damage = 10;
    [HideInInspector]
    public Photon.Realtime.Player owner;

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (rigidBody != null)
            print("Rigidbody is found!");
        else
            print("Rigidbody isn't found!");
    }

    public void InitializeBullet(Vector3 originalDirection, Photon.Realtime.Player givenPlayer)
    {
        transform.forward = originalDirection;
        rigidBody.velocity = transform.forward * bulletSpeed;
        owner = givenPlayer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Play audio and VFX for bullet hit
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play3D(BulletHitAudio, transform.position);
        }

        if (VFXManager.Instance != null)
        {
            VFXManager.Instance.PlayVFX(bulletImpactEffect, transform.position);
        }

        // Destroy the bullet on collision
        Destroy(gameObject);
    }
}
