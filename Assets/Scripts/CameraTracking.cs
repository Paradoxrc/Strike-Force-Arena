using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public Transform target; // The player to follow
    public Vector3 offset; // Offset to keep the camera at a good distance
    public float followSpeed = 5f; // Speed at which the camera follows

    void LateUpdate()
    {
        if (target != null)
        {
            // Smoothly follow the target
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            // Optionally look at the player
            transform.LookAt(target);
        }
    }







    // public Transform playerCharacter;

    // Vector3 cameraOffset;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     cameraOffset = transform.position - playerCharacter.position;
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     transform.position = playerCharacter.position + cameraOffset;
    // }
}
