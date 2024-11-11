using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class Multiplayer : MonoBehaviour, IPunObservable
{
    public float movementSpeed = 10f;
    private Rigidbody rigidbody;

    public float fireRate = 0.75f;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public GameObject bulletFiringEffect;
    private float nextFire;

    [HideInInspector]
    public int health = 100;
    public Slider healthBar;
    public Text playerNameText; // Reference for player name display

    public AudioClip playerShootingAudio;

    private PhotonView photonView;

    // Variables for networked synchronization
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();

        // Set the player's name
        playerNameText.text = photonView.Owner.NickName;

        // Ensure the name text is visible for all players
        playerNameText.color = photonView.IsMine ? Color.green : Color.red;

        // Initialize network position and rotation
        networkPosition = transform.position;
        networkRotation = transform.rotation;

        if (photonView.IsMine)
        {
            // Set the camera to follow this player
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                CameraTracking cameraFollow = mainCamera.GetComponent<CameraTracking>();
                if (cameraFollow != null)
                {
                    cameraFollow.target = transform;
                }
            }
        }
        else
        {
            // Non-local player settings
            rigidbody.isKinematic = false;
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Move();
            if (Input.GetKey(KeyCode.Space))
            {
                photonView.RPC("Fire", RpcTarget.AllViaServer);
            }
        }
        else
        {
            // Smooth interpolation for non-local players to prevent falling
            rigidbody.MovePosition(Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * 10));
            rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * 10));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send data to other clients
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(health);
        }
        else
        {
            // Receive data from the network
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            health = (int)stream.ReceiveNext();
            healthBar.value = health;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            MultiplayerBulletController bullet = collision.gameObject.GetComponent<MultiplayerBulletController>();
            TakeDamage(bullet);
        }
    }

    void TakeDamage(MultiplayerBulletController bullet)
    {
        health -= bullet.damage;
        healthBar.value = health;
        if (health <= 0)
        {
            bullet.owner.AddScore(1);
            PlayerDied();
        }
    }

    void PlayerDied()
    {
        health = 100;
        healthBar.value = health;
    }

    void Move()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Quaternion rotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0, verticalInput));
        transform.rotation = rotation;

        Vector3 movementDir = transform.forward * Time.deltaTime * movementSpeed;
        rigidbody.MovePosition(rigidbody.position + movementDir);
    }

    [PunRPC]
    void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
            bullet.GetComponent<MultiplayerBulletController>()?.InitializeBullet(transform.rotation * Vector3.forward, photonView.Owner);

            AudioManager.Instance.Play3D(playerShootingAudio, transform.position);
            VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);
        }
    }
}
